using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Data.Models.Booking;
using ETG.Data.Tour;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.Booking
{
    public class CartBookingSummary
    {
        public CartBookingSummary(double commisionPercentage, CurrentCurrencyPricing currentCurrencyPricing)
        {
            CommissionPercentage = commisionPercentage;
            CurrentCurrencyPricing = currentCurrencyPricing;
        }
        public string BookNowDisclaimer { get; set; }
        public bool IsOnSaleNow { get; set; }
        public bool OnSaleFullPaymentRequired { get; set; }
        public bool TourHasPeaceOfMind { get; set; }
        public string TourName { get; set; }
        public string TourCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime PaymentInstalmentDate { get; set; }
        public DateTime PaymentBalanceDueDate { get; set; }
        public int ChangeOfMindThresholdDays { get; set; }
        public double SecondInstalmentPercentage { get; set; }
        public string DepartureDateDisplay
        {
            get
            {
                if (DepartureDate == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return DepartureDate.ToString("dddd , dd MMM yyyy");
            }
        }
        public string CurrencySymbol { get; set; }
        public ItemBreakdown Packages { get; set; }
        public List<string> SelectedTwinShareRoomsType { get; set; }
        public ItemBreakdown SinglePackages { get; set; }
        public List<RoomOptionItemBreakdown> RoomOptions { get; set; }
        public List<ItemBreakdown> ExtraOptions { get; set; }
        public List<ItemBreakdown> AdditionalServices { get; set; }
  
        public PrePostNightsDetails PreNightsDetails { get; set; }
        public PrePostNightsDetails PostNightsDetails { get; set; }
        public double TwinPreNightBasePrice { get; set; }
        public double TwinPostNightBasePrice { get; set; }
        public double SinglePreNightBasePrice { get; set; }
        public double SinglePostNightBasePrice { get; set; }
        public int TotalPersons => Packages.Count + SinglePackages.Count;

        public bool HasMainHotel { get; set; }
        public double PackagesBreakdownTotalPrice
        {
            get
            {
                if (Packages == null || Packages.Count == 0)
                {
                    return 0;
                }
                var packagesTotalPrice = Packages.TotalPrice;
                if (PreNightsDetails != null)
                {
                    packagesTotalPrice += PreNightsDetails.NumberOfNights * Packages.Count * TwinPreNightBasePrice;
                }
                
                if (PostNightsDetails != null)
                {
                    packagesTotalPrice += PostNightsDetails.NumberOfNights * Packages.Count * TwinPostNightBasePrice;
                }

                return packagesTotalPrice;
            }
        }
        
        public double PackageUnitPrice
        {
            get
            {
                if (Packages == null || Packages.Count == 0)
                {
                    return 0;
                }
                return Math.Round(PackagesBreakdownTotalPrice / Packages.Count);
                
            }
        }
        
        public double GetPackageAdjustedUnitPrice(double totalAdjustmentPrice)
        {
            if (Packages == null || Packages.Count == 0)
            {
                return 0;
            }
            
            if (totalAdjustmentPrice == 0)
            {
                return PackageUnitPrice;
            }

            return PackageUnitPrice - Math.Round(totalAdjustmentPrice / Packages.Count);
        }
        public double GetPackageAdjustedTotalPrice(double adjustmentPrice)
        {
            if (Packages == null || Packages.Count == 0)
            {
                return 0;
            }
            
            return GetPackageAdjustedUnitPrice(adjustmentPrice) * Packages.Count;
        } 
        
        
        public double SinglePackagesBreakdownTotalPrice
        {
            get
            {
                if (SinglePackages == null || SinglePackages.Count == 0)
                {
                    return 0;
                }
                var packagesTotalPrice = SinglePackages.TotalPrice;
                if (PreNightsDetails != null)
                {
                    packagesTotalPrice += PreNightsDetails.NumberOfNights * SinglePackages.Count * SinglePreNightBasePrice;
                }
                
                if (PostNightsDetails != null)
                {
                    packagesTotalPrice += PostNightsDetails.NumberOfNights * SinglePackages.Count * SinglePostNightBasePrice;
                }

                return packagesTotalPrice;
            }
        }
        
        public double SinglePackageUnitPrice
        {
            get
            {
                if (SinglePackages == null || SinglePackages.Count == 0)
                {
                    return 0;
                }
                return Math.Round(SinglePackagesBreakdownTotalPrice / SinglePackages.Count);
                
            }
        }
        
        public double GetSinglePackageAdjustedUnitPrice(double totalAdjustmentPrice)
        {
            if (SinglePackages == null || SinglePackages.Count == 0)
            {
                return 0;
            }
            
            if (totalAdjustmentPrice == 0)
            {
                return SinglePackageUnitPrice;
            }

            return SinglePackageUnitPrice - Math.Round(totalAdjustmentPrice / SinglePackages.Count);
        }
        public double GetSinglePackageAdjustedTotalPrice(double adjustmentPrice)
        {
            if (SinglePackages == null || SinglePackages.Count == 0)
            {
                return 0;
            }
            
            return GetSinglePackageAdjustedUnitPrice(adjustmentPrice) * SinglePackages.Count;
        } 

        public double RoomOptionsTotalPrice
        {
            get
            {
                if (RoomOptions == null || RoomOptions.Count == 0)
                {
                    return 0;
                }
                
                return RoomOptions.Select(a => a.TotalPriceWithPrePostNights).Sum();
            }
        }
        

        public double ExtraOptionsTotalPrice
        {
            get
            {
                if (ExtraOptions == null)
                {
                    return 0;
                }

                return ExtraOptions.Select(a => a.TotalPrice).Sum();
            }
        }
        public PromotionItem Promotion
        {
            get;
            set;
        }
        /*public ItemBreakdown EntireFlexPricing { get; set; }*/
        public DueDeposit Due
        {
            get;
            set;
        }

        public double GetPriceBeforeDiscount(CurrentCurrencyPricing currentCurrencyPricing, bool includeAdditionalService = true)
        {
            
            var packagePrice = PackagesBreakdownTotalPrice;
            var singleRoomPrice = SinglePackagesBreakdownTotalPrice;
            var roomOptionsPrice = RoomOptionsTotalPrice;
            var extrasPrice = ExtraOptions?.Select(a => a.TotalPrice).Sum();
            var additionalServicesPrice = AdditionalServices?.Select(a => a.TotalPrice).Sum();

            if (!currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                additionalServicesPrice = currentCurrencyPricing.ConvertCurrentCurrencyToAUD(additionalServicesPrice.GetValueOrDefault());
            }
            
            return packagePrice + singleRoomPrice + roomOptionsPrice +
                   extrasPrice.GetValueOrDefault() + (includeAdditionalService ? additionalServicesPrice.GetValueOrDefault() : 0);// + flexPrice.GetValueOrDefault();
        }
        
        public double Discount
        {
            get
            {
                if (CurrentCurrencyPricing == null || CurrentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    if (Promotion == null)
                    {
                        return 0;
                    }

                    return Promotion.GetDiscount(GetPriceBeforeDiscount(CurrentCurrencyPricing), TotalPersons);
                }

                return 0;
            }
        }
        public double TotalPrice
        {
            get
            {
                var priceBeforeDiscount=  GetPriceBeforeDiscount(CurrentCurrencyPricing);
                if (CurrentCurrencyPricing == null || CurrentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    return Math.Round(Promotion?.GetDiscountedPrice(priceBeforeDiscount, TotalPersons) ??
                                      priceBeforeDiscount);   
                }

                return priceBeforeDiscount;
            }
        }

        public double TotalPriceMinusAddedServices
        {
            get
            {
                var priceBeforeDiscount=  GetPriceBeforeDiscount(CurrentCurrencyPricing, false);
                if (CurrentCurrencyPricing == null || CurrentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    return Math.Round(Promotion?.GetDiscountedPrice(priceBeforeDiscount, TotalPersons) ??
                                      priceBeforeDiscount);   
                }

                return priceBeforeDiscount;
            }
        }

        public double TotalPriceInAUD { get; set; }

        private double CommissionPercentage { get; }

        public double TotalNetPrice => Math.Round(TotalPrice -  (TotalPrice * CommissionPercentage * .01));
        public double TotalNetPriceInAUD { get; set; }
        public double AgentDefinedPrice { get; set; }
        public double AgentDefinedPriceInAUD { get; set; }

        public double GetTotalNetPrice(double grossPrice)
        {
            return Math.Round(grossPrice -  (grossPrice * CommissionPercentage * .01));
        }
        public double AgentSellPrice => AgentDefinedPrice > 0 ? AgentDefinedPrice : TotalPrice;

        public double AgentPriceDifference
        {
            get
            {
                if (AgentDefinedPrice == 0)
                {
                    return 0;
                }

                return  TotalPrice - AgentDefinedPrice;
            }
        }
        
        public double AgentCommissionAmount => AgentSellPrice - TotalNetPrice;


        public double GetAgentPriceDifference(double agentDefinePrice)
        {
            if (agentDefinePrice == 0)
            {
                return 0;
            }

            return  TotalPrice - agentDefinePrice;
        
        }

        public double SecondInstalmentPrice
        {
            get
            {
                if (PaymentInstalmentDate == DateTime.MinValue)
                {
                    return 0;
                }

                return TotalPrice * SecondInstalmentPercentage * 0.01;
            }
        }

        public CurrentCurrencyPricing CurrentCurrencyPricing { get; set; }
        public int TourNumberOfNights { get; set; }


        public void ConvertPricesToCurrentCurrency(CurrentCurrencyPricing currentCurrencyPricing, CurrentCurrencyPricing audCurrencyPricing = null)
        {
            if (currentCurrencyPricing == null)
            {
                return;
            }
            CurrentCurrencyPricing = currentCurrencyPricing;

            //Agent defined price is the only price in current currency, all prices are in AUD
            AgentDefinedPriceInAUD = currentCurrencyPricing.ConvertCurrentCurrencyToAUD(AgentDefinedPrice);
            
            if (audCurrencyPricing != null)
            {
                currentCurrencyPricing = audCurrencyPricing;
            }

            TotalPriceInAUD = TotalPrice;
            TotalNetPriceInAUD = TotalNetPrice;
            
            if (Due != null)
            {
                Due.TotalDepositeInAUD = Due.TotalDeposit;   
            }
            if (currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                return;
            }

            if (Packages != null)
            {
                Packages.UnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(Packages.UnitPrice);
            }

            if (SinglePackages != null)
            {
                SinglePackages.UnitPrice  = currentCurrencyPricing.ConvertAUDToCurrentCurrency(SinglePackages.UnitPrice);
            }
            if (!RoomOptions.IsNullOrEmpty())
            {
                RoomOptions.ForEach(a => a.UnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.UnitPrice));
                RoomOptions.ForEach(a=> a.UnitPriceWithPrePostNights = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.UnitPriceWithPrePostNights));
                RoomOptions.ForEach(a=> a.PreNightUnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.PreNightUnitPrice));
                RoomOptions.ForEach(a=> a.PostNightUnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.PostNightUnitPrice));
            }

            if (!ExtraOptions.IsNullOrEmpty())
            {
                ExtraOptions.ForEach(a=> a.UnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.UnitPrice));
            }

            TwinPreNightBasePrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(TwinPreNightBasePrice);
            TwinPostNightBasePrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(TwinPostNightBasePrice);
            SinglePreNightBasePrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(SinglePreNightBasePrice);
            SinglePostNightBasePrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(SinglePostNightBasePrice);
            
            /*
             Additional service are entered in CMS with current currency in mind
             if (!AdditionalServices.IsNullOrEmpty())
            {
                AdditionalServices.ForEach(a=> a.UnitPrice = currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.UnitPrice));
            }*/

            if (Due?.DepositPrice != null)
            {
                Due.DepositPrice.UnitPrice =
                    currentCurrencyPricing.ConvertAUDToCurrentCurrency(Due.DepositPrice.UnitPrice);
            }

            if (Promotion != null)
            {
                if (currentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    Promotion.PromotionDollarDiscount =
                        currentCurrencyPricing.ConvertAUDToCurrentCurrency(Promotion.PromotionDollarDiscount);   
                }
                else
                {
                    Promotion = null;
                }
            }
        }

        private double GetTotalPriceInAUD(CurrentCurrencyPricing currentCurrencyPricing)
        {
            
            var packagePrice = PackagesBreakdownTotalPrice;
            var singleRoomPrice = SinglePackagesBreakdownTotalPrice;
            var roomOptionsPrice = RoomOptionsTotalPrice;
            var extrasPrice = ExtraOptions?.Select(a => a.TotalPrice).Sum();
            
            var additionalServicesPrice = AdditionalServices?.Select(a => a.TotalPrice).Sum() ?? 0;

            if (additionalServicesPrice > 0 && currentCurrencyPricing != null && !currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                additionalServicesPrice = currentCurrencyPricing.ConvertCurrentCurrencyToAUD(additionalServicesPrice);
            }    
            
            var priceBeforeDiscount = packagePrice + singleRoomPrice + roomOptionsPrice +
                   extrasPrice.GetValueOrDefault() + additionalServicesPrice;

            if (currentCurrencyPricing.CurrencyAppliesDiscounts)
            {
                return Math.Round(Promotion?.GetDiscountedPrice(priceBeforeDiscount, TotalPersons) ??
                                  priceBeforeDiscount);   
            }

            return priceBeforeDiscount;
        }
    }
}
