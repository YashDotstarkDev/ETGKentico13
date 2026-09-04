using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using CMS.EventLog;
using CMS.Helpers;
using CMSApp.Custom.Models;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace CMSApp.Custom.ETGBooking.Uploader
{
    public class PricingCSVValidator
    {
        private readonly List<TourBookingAdditions> _tourBookingAdditions;
        private CSVValidatorResult ValidationResult;
        private List<UploaderBookingAdditions> RoomUpgradesForUpdate;
        private List<UploaderBookingAdditions> OptionalExtrasForUpdate;
        private int CurrentRowNumber;
        private string CurrentTourCode;
        private bool IsValid;
        public PricingCSVValidator(List<TourBookingAdditions> additions)
        {
            _tourBookingAdditions = additions;
            ValidationResult = new CSVValidatorResult();
            RoomUpgradesForUpdate = new List<UploaderBookingAdditions>();
            OptionalExtrasForUpdate = new List<UploaderBookingAdditions>();
        }

        private string RowMessage(string message)
        {
            return $"Row {CurrentRowNumber} ({CurrentTourCode}): {message}";
        }

        public CSVValidatorResult Validate(List<BookingPriceImportModel> csvRows)
        {
            
            var rowFailedCount = 0;
            CurrentRowNumber = 1;
            foreach (var record in csvRows)
            {
                CurrentRowNumber++;
                CurrentTourCode = record.PackageCode;
                if (CurrentTourCode.IsNullOrEmpty())
                {
                    continue;
                }
                var startDate = UploaderHelpers.GetDateTime(record.StartDate);

                if (startDate == DateTime.MinValue)
                {
                    rowFailedCount++;
                    ValidationResult.Errors.Add(RowMessage("Invalid start date"));
                    continue;
                }

                var endDate = UploaderHelpers.GetDateTime(record.EndDate);

                if (endDate == DateTime.MinValue)
                {
                    rowFailedCount++;
                    ValidationResult.Errors.Add(RowMessage("Invalid end date"));
                    continue;
                }
                
                var additions = _tourBookingAdditions.Where(a => a.tourCode == record.PackageCode).FirstOrDefault();

                ValidateRoomUpgrades(record, additions);
                ValidateExtras(record, additions);
                
                
            }

            if (!csvRows.Any())
            {
                ValidationResult.Errors.Add("No Rows Processed.");
            }
            IsValid = ValidationResult.Success;
            return ValidationResult;
        }

        
        private void ValidateRoomUpgrades( BookingPriceImportModel record, TourBookingAdditions additions)
        {
            var validIndices = new List<(int index, string label)>();
            for (var i = 1; i <= 5; i++)
            {
                Type recordType = record.GetType();

                var labelValue =
                    ValidationHelper.GetString(recordType.GetProperty($"Twin{i}Label")?.GetValue(record),
                        string.Empty).Trim();
                double ppValue =
                    ValidationHelper.GetDouble(recordType.GetProperty($"Twin{i}PP")?.GetValue(record), 0);

                if (!labelValue.IsNullOrEmpty() && !labelValue.Equals("0") && ppValue > 0)
                {
                    validIndices.Add((i, labelValue));
                }

            }
            if (validIndices.Count == 0)
            {
                return;
            }

            var count = 0;

            if (additions != null && !additions.RoomUpgrades.IsNullOrEmpty())
            {
                count = additions.RoomUpgrades.Count;
            }

            if (validIndices.Count != count)
            {
                 ValidationResult.Errors.Add(RowMessage($"Number of Room upgrades in CMS and CSV doesn't match {validIndices.Count} (CSV) <> {count} (CMS)"));
            }
            else
            {
                for (var i = 0; i < validIndices.Count; i++)
                {
                    if (validIndices[i].index != additions.RoomUpgrades[i].RoomUpgradeNumber)
                    {
                        ValidationResult.Errors.Add(RowMessage("Numbering of Room upgrades doesn't match the CSV"));
                    }
                    else
                    {
                        if (additions.RoomUpgrades[i].RoomUpgradeTitle.Trim() != validIndices[i].label.Trim()
                            && !RoomUpgradesForUpdate.Any(a => a.NodeID == additions.RoomUpgrades[i].NodeID))
                        {
                            RoomUpgradesForUpdate.Add(new UploaderBookingAdditions
                            {
                                TourCode = additions.tourCode,
                                CMSLabel = additions.RoomUpgrades[i].RoomUpgradeTitle,
                                NodeAliasPath = additions.RoomUpgrades[i].NodeAliasPath,
                                NodeID = additions.RoomUpgrades[i].NodeID,
                                CSVLabel = validIndices[i].label,
                            });
                        }
                    }
                }
            }
            
        }

        private void ValidateExtras( BookingPriceImportModel record, TourBookingAdditions additions)
        {
            var validIndices = new List<(int index, string label)>();
            for (var i = 1; i <= 8; i++)
            {
                Type recordType = record.GetType();

                var labelValue =
                    ValidationHelper.GetString(recordType.GetProperty($"Extra{i}Label")?.GetValue(record),
                        string.Empty).Trim();
                double ppValue =
                    ValidationHelper.GetDouble(recordType.GetProperty($"Extra{i}PP")?.GetValue(record), 0);

                if (!labelValue.IsNullOrEmpty() && !labelValue.Equals("0") && ppValue > 0)
                {
                    validIndices.Add((i, labelValue));
                }

            }

            if (validIndices.Count == 0)
            {
                return;
            }

            var count = 0;

            if (additions != null && !additions.OptionalExtras.IsNullOrEmpty())
            {
                count = additions.OptionalExtras.Count;
            }

            if (validIndices.Count != count)
            {
                ValidationResult.Errors.Add(RowMessage("Number of Optional extras in CMS and CSV doesn't match"));
            }
            else
            {
                for (var i = 0; i < validIndices.Count; i++)
                {
                    if (validIndices[i].index != additions.OptionalExtras[i].OptionalExtrasNumber)
                    {
                        ValidationResult.Errors.Add(RowMessage("Numbering of Optional extras doesn't match the CSV"));
                    }
                    else
                    {
                        if (additions.OptionalExtras[i].OptionalExtrasTitle.Trim() != validIndices[i].label.Trim()
                        && !OptionalExtrasForUpdate.Any(a =>a.NodeID == additions.OptionalExtras[i].NodeID))
                        {

                            OptionalExtrasForUpdate.Add(new UploaderBookingAdditions
                            {
                                TourCode = additions.tourCode,
                                CMSLabel = additions.OptionalExtras[i].OptionalExtrasTitle,
                                NodeAliasPath = additions.OptionalExtras[i].NodeAliasPath,
                                NodeID = additions.OptionalExtras[i].NodeID,
                                CSVLabel = validIndices[i].label,
                            });
                        }
                    }
                }
            }
        }


        public (List<UploaderBookingAdditions> RoomUpgradesForUpgrade, List<UploaderBookingAdditions>
            OptionalExtrasForUpgrade)
            GetBookingAdditionsForUpgrades()
        {
            if (!IsValid)
            {
                throw new Exception("Not validated. Please call method Validate to validate.");
            }

            return (RoomUpgradesForUpdate, OptionalExtrasForUpdate);
        }
    }
}