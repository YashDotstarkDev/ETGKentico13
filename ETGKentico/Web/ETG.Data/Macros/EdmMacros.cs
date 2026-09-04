using System;
using Castle.Core.Internal;
using CMS;
using CMS.Base;
using CMS.Core;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.SiteProvider;
using CommonServiceLocator;
using Devotion.Web.Base.Extensions;
using ETG.Core.Extensions;
using ETG.Data.Experience.Models;
using ETG.Data.Experience.Services;
using ETG.Data.Macros;
using ETG.Data.Models.Modules;
using ETG.Data.Repositories.Modules;
using ETG.Data.Tour.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETG.Data.Helpers;
using ETG.Data.Models.Common;
using ETG.Data.Promotion;
using ETG.Data.Repositories.Image;
using ETG.Data.Tour.Models;

// Makes all methods in the 'CustomMacroMethods' container class available for string objects
[assembly: RegisterExtension(typeof(EdmMacros), typeof(string))]
// Registers methods from the 'CustomMacroMethods' container into the "String" macro namespace
[assembly: RegisterExtension(typeof(EdmMacros), typeof(StringNamespace))]

namespace ETG.Data.Macros
{
    public class EdmMacros : MacroMethodContainer
    {
        [MacroMethod(typeof(string), "return Imgix", 1)]
        [MacroMethodParam(0, "path", typeof(bool), "image path")]
        public static object Imgixify(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters[0] == null)
            {
                return string.Empty;
            }


            var path = parameters[0].ToString();
            return path.Imgixify();
        }


        [MacroMethod(typeof(string), "return Image with Pom", 4)]
        [MacroMethodParam(0, "path", typeof(string), "image path")]
        [MacroMethodParam(1, "hasPOMLogo", typeof(bool), "has POM logo")]
        [MacroMethodParam(2, "imageWidth", typeof(bool), "Image Width")]
        [MacroMethodParam(3, "imageHeight", typeof(bool), "Image Height")]
        [MacroMethodParam(4, "blendPad", typeof(bool), "Blend Pad")]
        [MacroMethodParam(5, "blendWidth", typeof(bool), "Blend Width")]
        public static object AddPomToImage(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length != 6 || parameters[1] == null)
            {
                return string.Empty;
            }

            var path = parameters[0].ToString();
            var hasPomLogo = ValidationHelper.GetBoolean(parameters[1], false);
            var imageWidth = ValidationHelper.GetInteger(parameters[2], 0);
            var imageHeight = ValidationHelper.GetInteger(parameters[3], 0);
            var blendPad = ValidationHelper.GetInteger(parameters[4], 0);
            var blendWidth = ValidationHelper.GetInteger(parameters[5], 0);

            return GetWidgetHeroImagePath(path, hasPomLogo, imageWidth, imageHeight, blendPad, blendWidth);
        }

        private static string GetWidgetHeroImagePath(string path, bool hasPomLogo, int imageWidth, int imageHeight,
            int blendPad, int blendWidth)
        {
            if (!hasPomLogo)
            {
                return path.Imgixify();
            }

            return
                $"{path.Imgixify(imageWidth, imageHeight)}&blend-mode=normal&blend-align=bottom,right&blend-w={blendWidth}&blend-pad={blendPad}&blend={"/edm/images/badge-peace-of-mind-booking-plan.png".ImgixifyNoParameter()}";
        }

        [MacroMethod(typeof(string), "return experiences html markup", 4)]
        [MacroMethodParam(0, "IsDarkBackground", typeof(bool), "Is Dark background")]
        [MacroMethodParam(1, "experienceguids", typeof(string), "experienceguids separated by ;")]
        [MacroMethodParam(2, "tourtypecodes", typeof(string), "tour type codes separated by ;")]
        [MacroMethodParam(3, "cruiseType", typeof(string), "cruise type")]
        [MacroMethodParam(4, "for feature tour", typeof(bool), "for feature tour")]
        public static object EdmExperiencesAndTravelStyles(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length < 4 ||
                (parameters[1] == null && parameters[2] == null && parameters[3] == null))
            {
                return string.Empty;
            }

            var html = new System.Text.StringBuilder();
            var isDarkBackground = ValidationHelper.GetBoolean(parameters[0], false);
            var experienceGuids = ValidationHelper.GetString(parameters[1], string.Empty);
            var tourTypeCodes = ValidationHelper.GetString(parameters[2], string.Empty);
            var cruiseTypeId = ValidationHelper.GetInteger(parameters[3], 0);
            var forFeatureTour = parameters.Length > 4 && ValidationHelper.GetBoolean(parameters[4], false);
            List<TourTypeModel> tourTypes = null;

            var fontcolor =
                isDarkBackground
                    ? "#ffffff"
                    : "#333333";

            if (!string.IsNullOrWhiteSpace(tourTypeCodes))
            {
                var tourTypeRepository = ServiceLocator.Current.GetInstance<ITourTypeRepository>();

                var codeList = tourTypeCodes.Split(';').ToList();
                tourTypes = tourTypeRepository.GetTourTypes(codeList).ToList();
            }


            if (!string.IsNullOrWhiteSpace(experienceGuids))
            {
                var guids = parameters[1].ToString().Split(';').Select(a => a.ToGuid()).ToList();
                var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();

                var experiences = experienceService.GetExperienceSummaries(guids);
                html.Append(GetExperienceMarkup(experiences, fontcolor, tourTypes.IsNullOrEmpty(), forFeatureTour,
                    isDarkBackground));
            }

            if (!tourTypes.IsNullOrEmpty() || cruiseTypeId > 0)
            {
                html.Append(GetTourTypesMarkup(tourTypes, cruiseTypeId, fontcolor, forFeatureTour, isDarkBackground));
            }

            return html.ToString();
        }

        private static string GetExperienceMarkup(IEnumerable<ExperienceSummaryModel> experiences, string fontcolor,
            bool hasBottomPadding, bool forFeatureTour, bool darkBackground)
        {
            var html = new System.Text.StringBuilder();
            if (!experiences.IsNullOrEmpty())
            {
                html.Append("<tr>");

                if (!hasBottomPadding)
                {
                    html.Append($"<td{FeatureTourClass(forFeatureTour)}  style=\"padding-top:11px;\">");
                }
                else
                {
                    html.Append(
                        $"<td{FeatureTourClass(forFeatureTour)} style=\"padding-top:11px; padding-bottom:30px\">");
                }

                html.Append("<table>");
                html.Append(
                    "<tr><td align=\"left\" style=\"font-size:14px; line-height:25px; font-family: Arial, sans-serif; font-weight:600;color:" +
                    fontcolor + "; padding-bottom:11px;\">Experience</td></tr>");
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                foreach (var experience in experiences)
                {
                    var image = experience.IconDarkImage.RemoveTilde();

                    if (darkBackground)
                    {
                        image = experience.IconWhiteImage.RemoveTilde();
                    }

                    html.Append("<tr>");
                    html.Append(
                        $"<td align=\"left\" style=\"padding-right:10px\"><img src=\"{image.Imgixify()}\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\" /></td>");
                    html.Append(
                        "<td align=\"left\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:" +
                        fontcolor + ";\">");
                    html.Append(experience.Name);
                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
            }

            return html.ToString();
        }

        private static string GetCruiseTypeMarkup(IEnumerable<TourSummaryInfoModel> cruiseTypeTours, string fontcolor,
            bool hasBottomPadding, bool forFeatureTour, bool darkBackground)
        {
            var html = new System.Text.StringBuilder();
            if (!cruiseTypeTours.IsNullOrEmpty())
            {
                html.Append("<tr>");

                if (!hasBottomPadding)
                {
                    html.Append($"<td{FeatureTourClass(forFeatureTour)}  style=\"padding-top:11px;\">");
                }
                else
                {
                    html.Append(
                        $"<td{FeatureTourClass(forFeatureTour)} style=\"padding-top:11px; padding-bottom:30px\">");
                }

                html.Append("<table>");
                html.Append(
                    "<tr><td align=\"left\" style=\"font-size:14px; line-height:25px; font-family: Arial, sans-serif; font-weight:600;color:" +
                    fontcolor + "; padding-bottom:11px;\">Cruise</td></tr>");
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                foreach (var tour in cruiseTypeTours)
                {
                    var image = tour.Images.FirstOrDefault()?.ImagePath.RemoveTilde();

                    html.Append("<tr>");
                    html.Append(
                        $"<td align=\"left\" style=\"padding-right:10px\"><img src=\"{image.Imgixify()}\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\" /></td>");
                    html.Append(
                        "<td align=\"left\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:" +
                        fontcolor + ";\">");
                    html.Append(tour.Name);
                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
            }

            return html.ToString();
        }

        private static string GetTourTypesMarkup(IEnumerable<TourTypeModel> tourTypes, int cruiseTypeId,
            string fontcolor, bool forFeatureTour, bool darkBackground)
        {
            var html = new System.Text.StringBuilder();
            if (!tourTypes.IsNullOrEmpty() || cruiseTypeId > 0)
            {
                html.Append("<tr>");
                html.Append($"<td{FeatureTourClass(forFeatureTour)} style=\"padding-top:11px; padding-bottom:30px\">");
                html.Append("<table>");
                html.Append(
                    "<tr><td align=\"left\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; font-weight:600;color:" +
                    fontcolor + "; padding-bottom:11px;\">Travel style</td></tr>");
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");

                if (!tourTypes.IsNullOrEmpty())
                {
                    foreach (var tourType in tourTypes)
                    {
                        var image = tourType.IconBlackImage.RemoveTilde();

                        if (darkBackground)
                        {
                            image = tourType.IconWhiteImage.RemoveTilde();
                        }

                        html.Append("<tr>");
                        html.Append(
                            $"<td align=\"left\" style=\"padding-right:10px\"><img src=\"{image.Imgixify()}\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\" /></td>");
                        html.Append(
                            "<td align=\"left\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:" +
                            fontcolor + ";\">");
                        html.Append(tourType.Name);
                        html.Append("</td>");
                        html.Append("</tr>");
                    }
                }

                if (cruiseTypeId > 0 && CruiseMapper.CruiseTypeMapping.ContainsKey(cruiseTypeId))
                {
                    var cruiseType = CruiseMapper.CruiseTypeMapping[cruiseTypeId];
                    var image = cruiseType.BlackIconImage;

                    if (darkBackground)
                    {
                        image = cruiseType.WhiteIconImage;
                    }

                    html.Append("<tr>");
                    html.Append(
                        $"<td align=\"left\" style=\"padding-right:10px\"><img src=\"{image.Imgixify()}\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\" /></td>");
                    html.Append(
                        "<td align=\"left\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:" +
                        fontcolor + ";\">");
                    html.Append(cruiseType.CruiseTypeName);
                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
            }

            return html.ToString();
        }

        private static string FeatureTourClass(bool forFeatureTour)
        {
            if (!forFeatureTour)
            {
                return string.Empty;
            }

            return " class=\"em_defaultlink em_aside15\"";
        }

        [MacroMethod(typeof(string), "return feature tour html markup", 4)]
        [MacroMethodParam(0, "tour code", typeof(string), "TourCode")]
        [MacroMethodParam(1, "heading", typeof(string), "heading")]
        [MacroMethodParam(2, "ctaCopy", typeof(string), "CTA Copy")]
        public static object FeatureTourAuto(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length != 3 || parameters[0] == null)
            {
                return string.Empty;
            }

            var tourService = ServiceLocator.Current.GetInstance<ITourService>();


            var tour = tourService.GetTourByTourCode(parameters[0].ToString());

            if (tour == null)
            {
                return string.Empty;
            }

            var heading = ValidationHelper.GetString(parameters[1], string.Empty);
            var ctaCopy = ValidationHelper.GetString(parameters[2], string.Empty);
            var ctaUrl = SiteContext.CurrentSite.SitePresentationURL + tour.TourSummaryInfo.Path;
            var html = new System.Text.StringBuilder();
            html.Append(
                "<table bgcolor=\"#333333\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"em_full_wrap\" style=\"table-layout:fixed; background-color:#333333;\">");
            html.Append("  <tr>");
            html.Append(
                "    <td align=\"center\" valign=\"top\"><table align=\"center\" width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"em_main_table\" style=\"width:600px; table-layout:fixed;\">");
            html.Append("        <tr>");
            html.Append(
                "          <td class=\"em_pad\" align=\"left\" valign=\"top\" style=\"padding:47px 0px 11px; font-size:20px; line-height:28px;  font-family: Arial, sans-serif; color:#ffffff; letter-spacing: 0.5px; text-transform:uppercase; font-weight:600;\">" +
                heading + "</td>");
            html.Append("        </tr>");
            html.Append("        <tr>");
            html.Append(
                "          <td class=\"em_pbottom\" align=\"left\" valign=\"top\" style=\"padding-bottom:25px;\"><img class=\"em_full_img\" src=\"" +
                GetWidgetHeroImagePath(tour.TourSummaryInfo?.Images.FirstOrDefault()?.ImagePath,
                    tour.TourSummaryInfo.HasPeaceOfMindGuarantee, 600, 345, 30, 150) + "\" alt=\"" +
                tour.TourSummaryInfo.Name +
                "\" border=\"0\" width=\"600\" height=\"345\" style=\"max-width:600px; display:block; font-size:16px; line-height:20px; font-weight:bold; color:#ffffff; font-family:Arial,sans-serif;\"/></td>");
            html.Append("        </tr>");
            html.Append("        <tr>");
            html.Append(
                "          <td class=\"em_defaultlink em_aside15\" align=\"left\" valign=\"top\" style=\"font-size:20px; line-height:28px;  font-family: Arial, sans-serif; color:#ffffff; letter-spacing: 0.5px; font-weight:500; padding-bottom:7px;\">" +
                tour.TourSummaryInfo.Name +
                $" <span style=\"font-size:10px; line-height:18px;  font-family: Arial, sans-serif; color:#ccc; font-weight:500\">({tour.TourSummaryInfo.TourCode})</span>" +
                "</td>");
            html.Append("        </tr>");
            html.Append("        <tr>");
            html.Append(
                "          <td class=\"em_defaultlink em_aside15 em_pbottom\" align=\"left\" valign=\"top\" style=\" font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#ffffff; padding-bottom:18px;\">" +
                tour.TourSummaryInfo.Summary + "</td>");
            html.Append("        </tr>");

            if (!tour.TourSummaryInfo.ExperienceGuids.IsNullOrEmpty())
            {
                var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();

                var experiences = experienceService.GetExperienceSummaries(tour.TourSummaryInfo.ExperienceGuids
                    .Split(';').Select(a => a.ToGuid()).ToList());

                html.Append(GetExperienceMarkup(experiences, "#ffffff", tour.TourSummaryInfo.TourTypes.IsNullOrEmpty(),
                    true, true));
            }

            if (!tour.TourSummaryInfo.TourTypes.IsNullOrEmpty() || tour.TourSummaryInfo.CruiseType > 0)
            {
                html.Append(GetTourTypesMarkup(tour.TourSummaryInfo.TourTypes, tour.TourSummaryInfo.CruiseType,
                    "#ffffff", true, true));
            }

            html.Append("        <tr>");
            html.Append("            <td style=\"border-top:2px solid #666;padding-bottom:25px\"></td>");
            html.Append("        </tr>");
            html.Append("        <tr>");
            html.Append(
                "          <td align=\"center\" valign=\"top\"><table align=\"left\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("              <tr>");
            html.Append(
                "                <td align=\"left\" valign=\"top\" class=\"em_p1\" style=\"padding-bottom:8px;\"><table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                    <tr>");
            html.Append(
                "                      <td align=\"left\" valign=\"middle\" style=\"font-size:14px; line-height:17px;  font-family: Arial, sans-serif; color:#ffffff;\">" +
                tour.TourSummaryInfo.NoOfNights + " nights</td>");
            html.Append(
                "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
            if (!tour.TourSummaryInfo.PriceInclusions.IsNullOrEmpty())
            {
                html.Append(
                    "                      <td align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:17px;  font-family: Arial, sans-serif; color:#ffffff;\">|</td>");


                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Flights") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon1.png?v=1\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Accommodation") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon2.png?v=1\" alt=\"\" border=\"0\" width=\"16\" style=\"max-width:16px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Meals") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon3.png?v=1\" alt=\"\" border=\"0\" width=\"11\" style=\"max-width:11px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Transfer") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon4.png?v=1\" alt=\"\" border=\"0\" width=\"13\" style=\"max-width:13px; display:block;\"/></td>");
                }
            }

            html.Append("                    </tr>");
            html.Append("                  </table></td>");
            html.Append("              </tr>");
            html.Append("            </table></td>");
            html.Append("        </tr>");
            if (tour.TourSummaryInfo.FromPrice != null)
            {
                html.Append("        <tr>");
                html.Append(
                    "          <td class=\"em_defaultlink em_aside15\" align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#929497;\">From <span style=\"color:#ffffff; font-weight:bold;\">" +
                    $"${tour.TourSummaryInfo.FromPrice.LowestPrice:#,###} {tour.TourSummaryInfo.PriceTypeLabel}" +
                    "</span></td>");
                html.Append("        </tr>");
            }

            html.Append("        <tr>");
            html.Append(
                "          <td class=\"em_defaultlink em_pbottom em_aside15\" align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#929497; padding-bottom:25px\">Travel period: " +
                tour.TravelDates + "</td>");
            html.Append("        </tr>");
            if (!string.IsNullOrEmpty(ctaCopy))
            {
                html.Append("        <tr>");
                html.Append(
                    "          <td class=\"em_pbottom\" align=\"left\" valign=\"top\" style=\"padding-bottom:44px;\"><table align=\"left\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                html.Append("              <tr>");
                html.Append(
                    "                <td align=\"center\" valign=\"top\"><table align=\"center\" bgcolor=\"#ca568e\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color:#ca568e; border-radius:30px;\">");
                html.Append("                    <tr>");
                html.Append(
                    "                      <td align=\"center\" valign=\"middle\" height=\"42\" style=\"height:42px; font-size:14px; color:#ffffff; font-family:Arial, sans-serif; text-transform:uppercase; padding:0px 29px; font-weight:600; letter-spacing: 0.5px;\"><a href=\"" +
                    ctaUrl +
                    "\" target=\"_blank\" style=\"text-decoration:none; color:#ffffff; display:block; line-height:42px;\">" +
                    ctaCopy + "</a></td>");
                html.Append("                    </tr>");
                html.Append("                  </table></td>");
                html.Append("              </tr>");
                html.Append("            </table></td>");
                html.Append("        </tr>");
            }

            html.Append("      </table></td>");
            html.Append("  </tr>");
            html.Append("</table>");

            return html.ToString();
        }

        private static List<ImageModel> GetGalleryImagesPlusHero(string path, ImageModel hero)
        {
            var imageRepository = ServiceLocator.Current.GetInstance<IImageRepository>();
            var images = imageRepository.GetImages(path);

            if (hero != null)
            {
                images.Insert(0, hero);
            }

            return images;
        }

        [MacroMethod(typeof(string), "return feature tour html markup", 7)]
        [MacroMethodParam(0, "tour code", typeof(string), "TourCode")]
        [MacroMethodParam(1, "header", typeof(string), "header")]
        [MacroMethodParam(2, "ctaCopy", typeof(string), "CTA Copy")]
        [MacroMethodParam(3, "primary image", typeof(string), "primary image")]
        //[MacroMethodParam(4, "thumbnail image 1", typeof(string), "thumbnail image 1")]
        //[MacroMethodParam(5, "thumbnail image 2", typeof(string), "thumbnail image 2")]
        //[MacroMethodParam(6, "thumbnail image 3", typeof(string), "thumbnail image 3")]
        public static object FeatureTourAutoWithGallery(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length < 3 || parameters[0] == null)
            {
                return string.Empty;
            }

            var tourService = ServiceLocator.Current.GetInstance<ITourService>();
            var tour = tourService.GetTourByTourCode(parameters[0].ToString());

            if (tour == null)
            {
                return string.Empty;
            }

            var promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
            tour.TourSummaryInfo.Promotion = promotionRepository.GetPromotionInfoForNonAgentForEDM(tour, DateTime.Today)
                .MapToPromotionItem();

            var heading = ValidationHelper.GetString(parameters[1], string.Empty);
            var ctaCopy = ValidationHelper.GetString(parameters[2], string.Empty);
            var ctaUrl = SiteContext.CurrentSite.SitePresentationURL + tour.TourSummaryInfo.Path;
            var primaryImage = ""; // ValidationHelper.GetString(parameters[3], string.Empty);

            if (parameters.Length > 3)
            {
                primaryImage = ValidationHelper.GetString(parameters[3], string.Empty);
            }

            var thumbImage1 = ""; //ValidationHelper.GetString(parameters[4], string.Empty);
            var thumbImage2 = ""; //ValidationHelper.GetString(parameters[5], string.Empty);
            var thumbImage3 = ""; //ValidationHelper.GetString(parameters[6], string.Empty);

            var tourExtraDetailService = ServiceLocator.Current.GetInstance<ITourExtraDetailService>();
            var inclusions = tourExtraDetailService.GetTourInclusions(tour.TourSummaryInfo.NodeAliasPath, true);

            var images = tour.TourSummaryInfo.Images.ToList();

            if (!images.IsNullOrEmpty())
            {
                if (primaryImage.IsNullOrEmpty())
                {
                    primaryImage = $"{images[0].ImagePath.RemoveTilde()}".Imgixify(600, 0);
                }
                else
                {
                    primaryImage = primaryImage.Imgixify(600, 0);
                }

                if (images.Count > 1)
                {
                    thumbImage1 = $"{images[1].ImagePath.RemoveTilde()}".Imgixify(300, 0);
                }

                if (images.Count > 2)
                {
                    thumbImage2 = $"{images[2].ImagePath.RemoveTilde()}".Imgixify(300, 0);
                }

                if (images.Count > 3)
                {
                    thumbImage3 = $"{images[3].ImagePath.RemoveTilde()}".Imgixify(300, 0);
                }
            }

            int blendWidth = 0;
            string logoImage;
            (logoImage, blendWidth) = GetLogoImage(tour);
            if (!logoImage.IsNullOrEmpty())
            {
                primaryImage =
                    $"{primaryImage}&blend-mode=normal&blend-align=top,left&blend-w={blendWidth}&blend-pad=20&blend={logoImage.ImgixifyNoParameter()}";
            }

            var html = new System.Text.StringBuilder();

            html.Append("<tr>");
            html.Append("       <td align=\"center\" valign=\"top\">");
            html.Append(
                "          <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                  <tr>");
            html.Append(
                "                    <td align=\"center\" valign=\"top\" style=\"padding: 46px 0px;\" class=\"em_pad\">");
            html.Append(
                "                      <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                        <tr>");
            html.Append(
                $"                          <td class=\"em_defaultlink em_f16\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 20px; line-height:24px; color:#FFFFFF; letter-spacing: 0.5px; font-weight:700; padding-bottom: 17px;\">{tour.TourSummaryInfo.Name}&nbsp;&nbsp;<span style=\"font-size: 10px; line-height: 13px; color: #929497; white-space: nowrap;\">(Package Code: {tour.TourSummaryInfo.TourCode})</span></td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                "                          <td align=\"center\" valign=\"top\" style=\"padding-bottom: 20px;\">");
            if (!primaryImage.IsNullOrEmpty())
            {
                html.Append(
                    "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
                html.Append("                              <tr>");
                html.Append(
                    $"		  <td align=\"center\" valign=\"top\" class=\"em_full_img\"><a href=\"{ctaUrl}\" target=\"_blank\" style=\"text-decoration: none; color: #000000;\"><img src=\"{primaryImage}\" width=\"600\" alt=\"{tour.TourSummaryInfo.Name}\" border=\"0\" style=\"max-width: 600px; display: block; font-size: 16px; line-height: 20px; font-family: Arial, sans-serif; color: #000000;\"/></a></td>");
                html.Append("	  </tr>");
                html.Append("                            </table>");
            }

            html.Append("                          </td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                "                          <td align=\"center\" valign=\"top\" style=\"padding-bottom: 25px;\">");
            html.Append(
                "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                              <tr>");
            if (!thumbImage1.IsNullOrEmpty())
            {
                html.Append(
                    $"                                <td align=\"center\" valign=\"top\" class=\"em_full_img\"><img src=\"{thumbImage1}\" width=\"187\" alt=\"\" border=\"0\" style=\"max-width: 187px; display: block;\"/></td>");
            }

            if (!thumbImage2.IsNullOrEmpty())
            {
                html.Append(
                    "                                <td width=\"20\" style=\"width: 20px;\" class=\"em_side15\">&nbsp;</td>");
                html.Append(
                    $"                                <td align=\"center\" valign=\"top\" class=\"em_full_img\"><img src=\"{thumbImage2}\" width=\"187\" alt=\"\" border=\"0\" style=\"max-width: 187px; display: block;\"/></td>");
            }


            if (!thumbImage3.IsNullOrEmpty())
            {
                html.Append(
                    "                                <td width=\"19\" style=\"width: 19px;\" class=\"em_side15\">&nbsp;</td>");
                html.Append(
                    $"                                <td align=\"center\" valign=\"top\" class=\"em_full_img\"><img src=\"{thumbImage3}\" width=\"187\" alt=\"\" border=\"0\" style=\"max-width: 187px; display: block;\"/></td>");
            }

            html.Append("                              </tr>");
            html.Append("                            </table>");
            html.Append("                          </td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");

            if (tour.TourSummaryInfo.Promotion == null)
            {
                html.Append(
                    $"                          <td class=\"em_defaultlink\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 16px; line-height:24px; color:#FFFFFF; padding-bottom: 10px;\">{tour.TourSummaryInfo.DisplayedCountryNames}</td>");
            }
            else
            {
                html.Append("<td align=\"center\" valign=\"top\" style=\"padding-bottom: 7px;\">");
                html.Append(
                    "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
                html.Append("                              <tr>");
                html.Append("                                <td>");
                html.Append(
                    "                                <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"170\" style=\"width: 170px;\" class=\"em_wrapper\">");
                html.Append("                                  <tr>");
                html.Append(
                    $"                                    <td class=\"em_defaultlink\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 16px; line-height:24px; color:#FFFFFF; padding-bottom: 10px;\">{tour.TourSummaryInfo.DisplayedCountryNames}</td>");
                html.Append("                                  </tr>");
                html.Append("                                </table>");
                html.Append("                              <!--[if gte mso 9]>");
                html.Append("                                      </td>");
                html.Append("                                      <td valign=\"top\">");
                html.Append("                                        <![endif]-->");
                html.Append(
                    "                                  <table  align=\"right\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"em_wrapper\" style=\"width:400px\">");
                html.Append("                                    <tr>");
                html.Append(
                    "                                    <td class=\"em_defaultlink\" align=\"right\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 14px; line-height:24px; color:#FFFFFF; padding-bottom: 10px;\">");
                html.Append(
                    "                                      <table align=\"right\" bgcolor=\"#CA568E\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color:#CA568E;\" class=\"em_wrapper\">");
                html.Append("                                        <tr>");
                html.Append(
                    "                                          <td style=\"font-family: Arial, sans-serif; font-size: 14px; line-height:24px; color:#FFFFFF; padding: 3px 5px;\">");
                html.Append(
                    $"                                            {tour.TourSummaryInfo.Promotion.PromotionName} | {tour.TourSummaryInfo.Promotion.BookByText}");
                html.Append("                                            </td>");
                html.Append("                                        </tr>");
                html.Append("                                      </table>");
                html.Append("                                    </td>");
                html.Append("                                    </tr>");
                html.Append("                                  </table>");
                html.Append("                                  </td>");
                html.Append("                              </tr>");
                html.Append("                              </table>");
                html.Append("                          </td>");
            }

            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                "                          <td align=\"center\" valign=\"top\" style=\"padding-bottom: 7px;\">");
            html.Append(
                "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                              <tr>");
            html.Append("                                <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                  <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"170\" style=\"width: 170px;\" class=\"em_wrapper\">");
            html.Append("                                    <tr>");
            html.Append(
                "                                      <td align=\"center\" valign=\"top\" class=\"em_pbottom\">");
            html.Append(
                "                                        <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" >");
            html.Append("                                          <tr>");
            html.Append(
                $"                                            <td class=\"em_defaultlink em_f12\" align=\"left\" valign=\"top\" style=\"font-family: 'Roboto', Arial, sans-serif; font-size: 14px; line-height:17px; color:#FFFFFF;\">{(tour.TourSummaryInfo.NoOfNights + 1)} days&nbsp;&nbsp;&nbsp;|</td>");
            if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Flights") > -1)
            {
                html.Append(
                    $"                                            <td width=\"13\" style=\"width: 13px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                            <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon1.png?v=1\" width=\"15\" alt=\"\" border=\"0\" style=\"max-width: 15px; display: block;\"/></td>");
            }

            if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Accommodation") > -1)
            {
                html.Append(
                    $"                                            <td width=\"13\" style=\"width: 13px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                            <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon2.png?v=1\" width=\"15\" alt=\"\" border=\"0\" style=\"max-width: 15px; display: block;\"/></td>");
            }

            if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Meals") > -1)
            {
                html.Append(
                    $"                                            <td width=\"12\" style=\"width: 12px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                            <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon3.png?v=1\" width=\"15\" alt=\"\" border=\"0\" style=\"max-width: 15px; display: block;\"/></td>");
            }

            if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Transfer") > -1)
            {
                html.Append(
                    $"                                            <td width=\"12\" style=\"width: 12px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                            <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon4.png?v=1\" width=\"15\" alt=\"\" border=\"0\" style=\"max-width: 15px; display: block;\"/></td>");
            }

            html.Append("                                          </tr>");
            html.Append("                                        </table>");
            html.Append("                                      </td>");
            html.Append("                                    </tr>");
            html.Append("                                  </table>");
            html.Append("   <!--[if gte mso 9]>");
            html.Append("                                      </td>");
            html.Append("                                      <td valign=\"top\">");
            html.Append("                                        <![endif]-->");
            html.Append(
                "                                  <table align=\"right\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"400\" style=\"width: 400px;\" class=\"em_wrapper\">");
            html.Append("                                    <tr>");
            html.Append("                                      <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                        <table align=\"right\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"em_wrapper\">");
            html.Append("                                          <tr>");
            //html.Append(
            //    "                                            <td width=\"125\" style=\"width: 125px;\" class=\"em_hide\">&nbsp;</td>");

            html.Append("                                            <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                              <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            html.Append("                                                <tr>");
            var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();

            var experiences = experienceService.GetExperienceSummaries(tour.TourSummaryInfo.ExperienceGuids.Split(';')
                .Select(a => a.ToGuid()).ToList());
            if (!experiences.IsNullOrEmpty())
            {
                var firstExperience = experiences.FirstOrDefault();
                html.Append(
                    $"                                                  <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}{firstExperience.IconWhiteImage.RemoveTilde()}\" width=\"18\" alt=\"\" border=\"0\" style=\"max-width: 18px; display: block;\"/></td>");
                html.Append(
                    $"                                                  <td width=\"5\" style=\"width: 5px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                                  <td class=\"em_defaultlink em_f12\" align=\"left\" valign=\"middle\" style=\"font-family: 'Roboto', Arial, sans-serif; font-size: 14px; line-height:17px; color:#FFFFFF;\">{firstExperience.Name}&nbsp;&nbsp;&nbsp;|</td>");
            }

            if (!tour.TourSummaryInfo.TourTypes.IsNullOrEmpty())
            {
                var tourType = tour.TourSummaryInfo.TourTypes[0];
                html.Append(
                    $"                                                  <td width=\"13\" style=\"width: 13px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                                  <td align=\"center\" valign=\"middle\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}{tourType.IconWhiteImage.RemoveTilde()}\" width=\"18\" alt=\"\" border=\"0\" style=\"max-width: 18px; display: block;\"/></td>");
                html.Append(
                    $"                                                  <td width=\"5\" style=\"width: 5px; font-size: 0px; line-height: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;\" border=\"0\"/></td>");
                html.Append(
                    $"                                                  <td class=\"em_defaultlink em_f12\" align=\"left\" valign=\"middle\" style=\"font-family: 'Roboto', Arial, sans-serif; font-size: 14px; line-height:17px; color:#FFFFFF;\">{tourType.Name}</td>");
            }

            html.Append("                                                </tr>");
            html.Append("                                              </table>");
            html.Append("                                            </td>");
            html.Append("                                          </tr>");
            html.Append("                                        </table>");
            html.Append("                                      </td>");
            html.Append("                                    </tr>");
            html.Append("                                  </table>");
            html.Append("                                </td>");
            html.Append("                              </tr>");
            html.Append("                            </table>");
            html.Append("                          </td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                $"                          <td class=\"em_defaultlink\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 14px; line-height:17px; color:#929497; padding-bottom: 18px;\">Travel period: {tour.TravelDates}</td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                $"                          <td class=\"em_defaultlink\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 16px; line-height:24px; color:#FFFFFF; padding-bottom: 12px;\">{tour.TourSummaryInfo.Summary}</td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append(
                "                          <td align=\"center\" valign=\"top\" style=\"padding-bottom: 20px;\">");
            html.Append(
                "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                              <tr>");
            html.Append(
                "                                <td width=\"6\" style=\"width: 6px; font-size: 0px; line-height: 0px;\">&nbsp;</td>");
            html.Append("                                <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                  <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            /*html.Append("                                    <tr>");
            html.Append(
                "                                      <td align=\"center\" valign=\"top\" style=\"padding-bottom: 8px;\">");
            html.Append(
                "                                        <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            html.Append("                                          <tr>");
            html.Append(
                "                                            <td width=\"12\" align=\"left\" valign=\"top\" style=\"width: 12px; font-size: 14px; line-height: 17px; font-family: Arial, sans-serif; color: #FFFFFF;\">&bull;</td>");
            html.Append(
                $"                                            <td align=\"left\" valign=\"top\" style=\"font-size: 14px; line-height: 17px; font-family: Arial, sans-serif; color: #FFFFFF;\">{GetTourFromTo(tour)}</td>");
            html.Append("                                          </tr>");
            html.Append("                                        </table>");
            html.Append("                                      </td>");
            html.Append("                                    </tr>");
            */

            foreach (var inclusion in inclusions)
            {
                html.Append("                                    <tr>");
                html.Append(
                    "                                      <td align=\"center\" valign=\"top\" style=\"padding-bottom: 8px;\">");
                html.Append(
                    "                                        <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                html.Append("                                          <tr>");
                html.Append(
                    "                                            <td width=\"12\" align=\"left\" valign=\"top\" style=\"width: 12px; font-size: 14px; line-height: 17px; font-family: Arial, sans-serif; color: #FFFFFF;\">&bull;</td>");
                html.Append(
                    $"                                            <td align=\"left\" valign=\"top\" style=\"font-size: 14px; line-height: 17px; font-family: Arial, sans-serif; color: #FFFFFF;\">{inclusion.Value}</td>");
                html.Append("                                          </tr>");
                html.Append("                                        </table>");
                html.Append("                                      </td>");
                html.Append("                                    </tr>");
            }

            html.Append("                                  </table>");
            html.Append("                                </td>");
            html.Append("                              </tr>");
            html.Append("                            </table>");
            html.Append("                          </td>");
            html.Append("                        </tr>");
            html.Append("                        <tr>");
            html.Append("                          <td align=\"center\" valign=\"top\">");
            html.Append(
                "                            <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("                              <tr>");
            html.Append("                                <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                  <table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"em_wrapper\">");
            html.Append("                                    <tr>");
            html.Append(
                "                                      <td align=\"center\" valign=\"top\" class=\"em_pbottom\">");
            html.Append(
                "                                        <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            html.Append("                                          <tr>");

            if (tour.TourSummaryInfo.FromPrice != null)
            {
                var priceText = tour.TourSummaryInfo.FromPrice.LowestPrice.ToString("#,###");
                var originalPriceHtml = "";
                if (tour.TourSummaryInfo.Promotion != null)
                {
                    priceText = tour.TourSummaryInfo.Promotion
                        .GetDiscountedPrice(tour.TourSummaryInfo.FromPrice.LowestPrice).ToString("#,###");
                    originalPriceHtml =
                        $"<span style=\"text-decoration:line-through;color:#ff0000;font-size:20px;padding-left:10px\">AUD{tour.TourSummaryInfo.FromPrice.LowestPrice:#,###}</span>";
                }

                html.Append(
                    $"<td class=\"em_defaultlink em_text1\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 28px; line-height:32px; color:#FFFFFF; padding-top: 12px; font-weight: 700;\"><span class=\"em_f12\" style=\"font-family:'Roboto', Arial, sans-serif; font-size: 14px; line-height:17px; color: #929497; font-weight: 400;\">From</span>{originalPriceHtml} AUD{priceText} <span class=\"em_f12\" style=\"font-family: 'Roboto', Arial, sans-serif; font-size: 14px; line-height:17px; color: #FFFFFF; font-weight: 400;\">{tour.TourSummaryInfo.PriceTypeLabel}</span></td>");
            }
            else
            {
                html.Append(
                    $"<td class=\"em_defaultlink em_text1\" align=\"left\" valign=\"top\" style=\"font-family: Arial, sans-serif; font-size: 28px; line-height:32px; color:#FFFFFF; padding-top: 12px; font-weight: 700;\"></td>");
            }

            html.Append("                                          </tr>");
            html.Append("                                        </table>");
            html.Append("                                      </td>");
            html.Append("                                    </tr>");
            html.Append("                                  </table>");
            html.Append("   <!--[if gte mso 9]>");
            html.Append("                                      </td>");
            html.Append("                                      <td valign=\"top\">");
            html.Append("                                        <![endif]-->");
            if (!string.IsNullOrEmpty(ctaCopy))
            {
                html.Append(
                    "                                  <table align=\"right\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"em_wrapper\">");
                html.Append("                                    <tr>");
                html.Append("                                      <td align=\"center\" valign=\"top\">");
                html.Append("                                        <div><!--[if mso]>");
                html.Append(
                    $"                                          <v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"{ctaUrl}\" style=\"height:40px;v-text-anchor:middle;width:180px;\" arcsize=\"50%\" stroke=\"f\" fillcolor=\"#CA568E\">");
                html.Append("                                            <w:anchorlock/>");
                html.Append("                                              <center>");
                html.Append("                                          <![endif]-->");
                html.Append(
                    $"                                           <a href=\"{ctaUrl}\" target=\"_blank\" style=\"background-color:#CA568E;border-radius:21px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:14px;font-weight:bold;line-height:42px;text-align:center;text-decoration:none;width:180px;-webkit-text-size-adjust:none;\"><svg width=\"11\" height=\"13\" viewBox=\"0 0 11 13\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M10.5 11.375C10.5 12.0078 10.0078 12.5 9.375 12.5H1.125C0.515625 12.5 0 12.0078 0 11.375V5H10.5V11.375ZM3 6.78125C3 6.64062 2.88281 6.5 2.71875 6.5H1.78125C1.64062 6.5 1.5 6.64062 1.5 6.78125V7.71875C1.5 7.88281 1.64062 8 1.78125 8H2.71875C2.88281 8 3 7.88281 3 7.71875V6.78125ZM3 9.78125C3 9.64062 2.88281 9.5 2.71875 9.5H1.78125C1.64062 9.5 1.5 9.64062 1.5 9.78125V10.7188C1.5 10.8828 1.64062 11 1.78125 11H2.71875C2.88281 11 3 10.8828 3 10.7188V9.78125ZM6 6.78125C6 6.64062 5.88281 6.5 5.71875 6.5H4.78125C4.64062 6.5 4.5 6.64062 4.5 6.78125V7.71875C4.5 7.88281 4.64062 8 4.78125 8H5.71875C5.88281 8 6 7.88281 6 7.71875V6.78125ZM6 9.78125C6 9.64062 5.88281 9.5 5.71875 9.5H4.78125C4.64062 9.5 4.5 9.64062 4.5 9.78125V10.7188C4.5 10.8828 4.64062 11 4.78125 11H5.71875C5.88281 11 6 10.8828 6 10.7188V9.78125ZM9 6.78125C9 6.64062 8.88281 6.5 8.71875 6.5H7.78125C7.64062 6.5 7.5 6.64062 7.5 6.78125V7.71875C7.5 7.88281 7.64062 8 7.78125 8H8.71875C8.88281 8 9 7.88281 9 7.71875V6.78125ZM9 9.78125C9 9.64062 8.88281 9.5 8.71875 9.5H7.78125C7.64062 9.5 7.5 9.64062 7.5 9.78125V10.7188C7.5 10.8828 7.64062 11 7.78125 11H8.71875C8.88281 11 9 10.8828 9 10.7188V9.78125ZM1.125 2H2.25V0.875C2.25 0.6875 2.4375 0.5 2.625 0.5H3.375C3.58594 0.5 3.75 0.6875 3.75 0.875V2H6.75V0.875C6.75 0.6875 6.9375 0.5 7.125 0.5H7.875C8.08594 0.5 8.25 0.6875 8.25 0.875V2H9.375C10.0078 2 10.5 2.51562 10.5 3.125V4.25H0V3.125C0 2.51562 0.515625 2 1.125 2Z\" fill=\"currentColor\"></path></svg>&nbsp;&nbsp;{ctaCopy}&nbsp;</a>");
                html.Append("                                          <!--[if mso]>");
                html.Append("                                            </center>");
                html.Append("                                          </v:roundrect>");
                html.Append("                                        <![endif]--></div>");
                html.Append("                                      </td>");
                html.Append("                                    </tr>");
                html.Append("                                  </table>");
            }

            html.Append("                                </td>");
            html.Append("                              </tr>");
            html.Append("                            </table>");
            html.Append("                          </td>");
            html.Append("                        </tr>");
            html.Append("                      </table>");
            html.Append("                    </td>");
            html.Append("                  </tr>");
            html.Append("                </table>");
            html.Append("              </td>");
            html.Append("            </tr>");
            return html.ToString();
        }

        private static (string, int) GetLogoImage(TourModel tour)
        {
            var logoImage = "";
            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourIsExclusive && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/POM-Exclusive-Safe-FOC_new.png";
                return (logoImage, 200);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"/images/POM-Exclusive-Safe_new.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/POM-Safe-FOC_new.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourIsExclusive &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/POM-Exclusive-FOC_new.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourIsExclusive &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/Exclusive-Safe-FOC_new.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/POM-FOC_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourIsExclusive && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/Exclusive-FOC_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"/images/Safe-FOC_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/POM-Safe_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/POM-Exclusive_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/Exclusive-Safe_new.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/PDF-POM_new.png";
                return (logoImage, 90);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/pdf-safe-travels_new.png";

                return (logoImage, 90);
            }

            if (tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/pdf-Exclusive-Packages_new.png";
                return (logoImage, 90);
            }

            if (tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/images/pdf-freedom-of-choice_new.png";
                return (logoImage, 90);
            }

            return (string.Empty, 0);
        }

        private static string GetTourFromTo(TourModel tour)
        {
            if (tour.TourSummaryInfo.DestinationCity.IsNullOrEmpty())
            {
                return tour.TourSummaryInfo.DepartureCity;
            }

            return $"{tour.TourSummaryInfo.DepartureCity} &gt; {tour.TourSummaryInfo.DestinationCity}";
        }


        [MacroMethod(typeof(string), "return tour html markup", 1)]
        [MacroMethodParam(0, "tour code", typeof(string), "TourCode")]
        public static object TourAuto(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters[0] == null)
            {
                return string.Empty;
            }

            var tourService = ServiceLocator.Current.GetInstance<ITourService>();


            var tour = tourService.GetTourByTourCode(parameters[0].ToString());

            if (tour == null)
            {
                return string.Empty;
            }

            var heroImage = tour.TourSummaryInfo.Images.FirstOrDefault();

            var ctaUrl = SiteContext.CurrentSite.SitePresentationURL + tour.TourSummaryInfo.Path;
            var html = new System.Text.StringBuilder();
            html.Append(
                "<table bgcolor=\"#ffffff\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"em_full_wrap\" style=\"table-layout:fixed; background-color:#ffffff;\">");
            html.Append("  <tr>");
            html.Append("    <td align=\"center\" valign=\"top\">");
            html.Append(
                "      <table align=\"center\" width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"em_main_table\" style=\"width:600px; table-layout:fixed;\">");
            html.Append("        <tr>");
            html.Append("          <td class=\"em_pad\" align=\"center\" valign=\"top\">");
            html.Append(
                "            <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("              <!-- == 2column Section1 == -->");
            html.Append("                <tr>");
            html.Append(
                "                  <td class=\"em_defaultlink em_p15\"  align=\"left\" valign=\"top\" style=\"font-size:20px; line-height:28px;  font-family: Arial, sans-serif; color:#CA568E; letter-spacing: 0.5px;  padding-bottom:6px; \">" +
                tour.TourSummaryInfo.Name +
                $" <span style=\"font-size:10px; line-height:18px;  font-family: Arial, sans-serif; color:#CA568E;\">({tour.TourSummaryInfo.TourCode})</span>" +
                "</td>");
            html.Append("                </tr>");
            html.Append("                <tr>");
            html.Append(
                "                  <td align=\"left\" valign=\"top\" style=\"padding-bottom:6px;\"><table width=\"165\" align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:165px;\">");
            html.Append("                    <tr>");
            html.Append(
                "                      <td align=\"left\" valign=\"middle\" style=\"font-size:14px; line-height:17px;  font-family: Arial, sans-serif; color:#333;\">" +
                tour.TourSummaryInfo.NoOfNights + " nights</td>");
            html.Append(
                "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
            if (!tour.TourSummaryInfo.PriceInclusions.IsNullOrEmpty())
            {
                html.Append(
                    "                      <td align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:17px;  font-family: Arial, sans-serif; color:#333;\">|</td>");
                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Flights") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon5.png?v=1\" alt=\"\" border=\"0\" width=\"15\" style=\"max-width:15px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Accommodation") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon6.png?v=1\" alt=\"\" border=\"0\" width=\"16\" style=\"max-width:16px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Meals") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon7.png?v=1\" alt=\"\" border=\"0\" width=\"11\" style=\"max-width:11px; display:block;\"/></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusions.IndexOf("Transfer") > -1)
                {
                    html.Append(
                        "                      <td align=\"left\" valign=\"top\" width=\"9\" style=\"width:9px; font-size:0px; line-height:0px;\"><img src=\"http://etgp0001.creative.madewithdevotion.com.au/edm/images/spacer.gif\" alt=\"\" border=\"0\" height=\"1\" width=\"1\" style=\"display:block;\"/></td>");
                    html.Append(
                        $"                      <td align=\"center\" valign=\"middle\" style=\"font-size:0px; line-height:0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images/icon8.png?v=1\" alt=\"\" border=\"0\" width=\"13\" style=\"max-width:13px; display:block;\"/></td>");
                }
            }

            html.Append("                  </tr>");
            html.Append("                  </table></td>");
            html.Append("                </tr>");

            if (tour.TourSummaryInfo.FromPrice != null)
            {
                html.Append("                <tr>");
                html.Append(
                    "                  <td class=\"em_defaultlink\" align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#929497; \">From <span style=\"color:#333333; font-weight:bold;\">" +
                    $"${tour.TourSummaryInfo.FromPrice.LowestPrice:#,###} {tour.TourSummaryInfo.PriceTypeLabel}" +
                    "</span></td>");
                html.Append("                </tr>");
            }

            html.Append("                <tr>");
            html.Append(
                "                  <td class=\"em_defaultlink em_pbottom\" align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#929497; padding-bottom:13px \">Travel Period: " +
                tour.TravelDates + "</td>");
            html.Append("                </tr>");
            html.Append("                <tr>");
            html.Append(
                "                  <td class=\"em_pbottom\" align=\"center\" valign=\"top\" style=\"padding-bottom:45px;\"><table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                    <tr>");
            html.Append(
                "                      <td align=\"left\" valign=\"top\"><table align=\"left\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                        <tr>");
            html.Append(
                "                          <td align=\"center\" valign=\"top\"><img class=\"em_full_img1\" src=\"" +
                GetWidgetHeroImagePath(heroImage?.ImagePath, tour.TourSummaryInfo.HasPeaceOfMindGuarantee, 291, 166, 20,
                    100) +
                "\" alt=\"\" border=\"0\" width=\"291\" style=\"max-width:291px; display:block; font-size:16px; line-height:20px; font-weight:bold; color:#000000; font-family:Arial,sans-serif;\"/></td>");
            html.Append("                        </tr>");
            html.Append("                        </table>");
            html.Append("                        ");
            html.Append("                        <!--[if gte mso 9]>");
            html.Append("</td>");
            html.Append("<td valign=\"top\">");
            html.Append("<![endif]-->");
            html.Append("                        ");
            html.Append(
                "                        <table align=\"right\" width=\"289\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:289px;\">");
            html.Append("                          <tr>");
            html.Append(
                "                            <td align=\"center\" valign=\"top\" class=\"em_ptop\"><table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                              <tr>");
            html.Append(
                "                                <td class=\"em_defaultlink em_pbottom\" align=\"left\" valign=\"top\" style=\"font-size:14px; line-height:25px;  font-family: Arial, sans-serif; color:#333333; padding-bottom:11px;\">" +
                tour.TourSummaryInfo.Summary + "</td>");
            html.Append("                              </tr>");

            if (!tour.TourSummaryInfo.ExperienceGuids.IsNullOrEmpty())
            {
                var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();

                var experiences = experienceService.GetExperienceSummaries(tour.TourSummaryInfo.ExperienceGuids
                    .Split(';').Select(a => a.ToGuid()).ToList());

                html.Append(GetExperienceMarkup(experiences, "#333333", tour.TourSummaryInfo.TourTypes.IsNullOrEmpty(),
                    false, false));
            }

            if (!tour.TourSummaryInfo.TourTypes.IsNullOrEmpty() || tour.TourSummaryInfo.CruiseType > 0)
            {
                html.Append(GetTourTypesMarkup(tour.TourSummaryInfo.TourTypes, tour.TourSummaryInfo.CruiseType,
                    "#333333", false, false));
            }

            html.Append("                              <tr>");
            html.Append(
                "                                <td  align=\"left\" valign=\"top\"><table align=\"left\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                                  <tr>");
            html.Append(
                "                                    <td align=\"center\" valign=\"top\"><table align=\"center\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                                      <tr>");
            html.Append(
                "                                        <td align=\"center\" valign=\"middle\" height=\"40\" style=\"height:40px; font-size:14px; color:#CA568E; font-family:Arial, sans-serif; text-transform:uppercase; padding:0px 40px; font-weight:600; letter-spacing: 0.5px; border-radius:30px; border:2px solid #ca568e; display:block; \"><a href=\"" +
                ctaUrl +
                "\" target=\"_blank\" style=\"text-decoration:none; color:#CA568E; display:block; line-height:40px;\">Enquire</a></td>");
            html.Append("                                      </tr>");
            html.Append("                                      </table></td>");
            html.Append("                                  </tr>");
            html.Append("                                  </table></td>");
            html.Append("                              </tr>");
            html.Append("                              </table></td>");
            html.Append("                          </tr>");
            html.Append("                        </table></td>");
            html.Append("                  </tr>");
            html.Append("                  </table></td>");
            html.Append("                </tr>");
            html.Append("                <!-- == //2column Section1 == --> ");
            html.Append("            </table>");
            html.Append("          </td>");
            html.Append("          </tr>");
            html.Append("      </table>");
            html.Append("    </td>");
            html.Append("  </tr>");
            html.Append("</table>");
            return html.ToString();
        }

        [MacroMethod(typeof(string), "return Image with bottom curve", 1)]
        [MacroMethodParam(0, "image", typeof(string), "image")]
        public static object AddBottomCurveToImage(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters[0] == null)
            {
                return string.Empty;
            }

            var image = parameters[0].ToString().RemoveTilde();
            return
                $"{image.Imgixify(600, 0)}&blend-mode=normal&blend-align=bottom,left&blend-w=600&blend-pad=0&blend={"/images/curve.png".ImgixifyNoParameter()}";
        }

        [MacroMethod(typeof(string), "return feature tour html markup (2024)", 4)]
        [MacroMethodParam(0, "tour code", typeof(string), "TourCode")]
        [MacroMethodParam(1, "heading", typeof(string), "IsFeatured")]
        [MacroMethodParam(2, "ctaLabel", typeof(string), "CTA Label")]
        [MacroMethodParam(3, "savingsMessage", typeof(string), "Savings Pill Message")]
        [MacroMethodParam(4, "timer", typeof(string), "Timer Url")]
        public static object FeatureTourAuto2024(EvaluationContext context, params object[] parameters)
        {
            if (parameters == null || parameters.Length < 2 || parameters[0] == null)
            {
                return string.Empty;
            }


            ITourService tourService;
            if (!RequestContext.CurrentDomain.Contains("cms."))
            {
                tourService = DependencyResolver.Current.GetService<ITourService>();
            }
            else
            {
                tourService = ServiceLocator.Current.GetInstance<ITourService>();
            }

            //var tourService = ServiceLocator.Current.GetInstance<ITourService>();


            var tour = tourService.GetTourByTourCode(parameters[0].ToString());

            if (tour == null)
            {
                return string.Empty;
            }

            string ctaLabel = "Book now";
            if (parameters.Length >= 3)
            {
                ctaLabel = string.IsNullOrWhiteSpace(parameters[2].ToString()) ? ctaLabel : parameters[2].ToString();
            }

            string savingsMessage = string.Empty;
            if (parameters.Length >= 4)
            {
                savingsMessage = string.IsNullOrWhiteSpace(parameters[3].ToString())
                    ? savingsMessage
                    : parameters[3].ToString();
            }

            string timerUrl = string.Empty;
            if (parameters.Length >= 5)
            {
                timerUrl = ValidationHelper.GetString(parameters[4], null);
            }

            var isFeatured = ValidationHelper.GetBoolean(parameters[1], false);

            //var fileUrl = parameters.Length > 2 ? parameters[2].ToString() : null;

            IPromotionRepository promotionRepository;
            if (!RequestContext.CurrentDomain.Contains("cms."))
            {
                promotionRepository = DependencyResolver.Current.GetService<IPromotionRepository>();
            }
            else
            {
                promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
            }


            tour.TourSummaryInfo.Promotion = promotionRepository.GetPromotionInfoForNonAgentForEDM(tour, DateTime.Today)
                .MapToPromotionItem();

            var html = new System.Text.StringBuilder();
            html.Append("<tr>");
            html.Append("   <td align=\"center\" valign=\"top\">");
            html.Append("     <table align=\"center\" width=\"800\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 800px;\" class=\"em_wrapper\">");
            html.Append("       <tbody>");
            html.Append("           <tr>");
            html.Append($"         <td align=\"center\" valign=\"top\" style=\"padding: 0px 130px;\" {(isFeatured ? "bgcolor=\"#EDEDED\"" : "")} class=\"em_aside24\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">");
            html.Append("               <tbody>");
            if (!string.IsNullOrWhiteSpace(timerUrl))
            {
                html.Append("      <tr>");
                html.Append(
                    "      <td height=\"30\" style=\"height: 30px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
                html.Append("      </tr>");


                html.Append("      <tr>");
                html.Append("         <td align=\"center\" valign=\"top\" bgcolor=\"#333333\" style=\"padding: 0px 15px; border-top-left-radius: 8px; border-top-right-radius: 8px;\">");
                html.Append("          <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                html.Append("            <tbody>");
                html.Append("              <tr>");
                html.Append("                  <td height=\"24\" style=\"height:24px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
                html.Append("              </tr>");
                html.Append("              <tr>");
                html.Append("                 <td align=\"center\" valign=\"top\">");
                html.Append($"                    <img src=\"{timerUrl}\" width=\"100%\" style=\"display: block; width: 100%;\" alt=\"motionmailapp.com\"></td>");
                html.Append("              </tr>");
                html.Append("              <tr>");
                html.Append("                 <td height=\"19\" style=\"height: 19px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
                html.Append("              </tr>");
                html.Append("            </tbody>");
                html.Append("         </table>");
                html.Append("        </td>");
                html.Append("     </tr>");
            }
            // html.Append("                  <tr>");
            // html.Append("                      <td height=\"30\" style=\"height: 30px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            // html.Append("                  </tr>");
            // if (isFeatured)
            // {
            //     html.Append("              <tr>");
            //     html.Append(
            //         "                          <td align=\"center\" valign=\"top\" bgcolor=\"#333333\" style=\"padding: 0px 15px; border-top-left-radius: 8px; border-top-right-radius: 8px;\"><table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            //     html.Append("                     <tbody>");
            //     html.Append("                        <tr>");
            //     html.Append(
            //         "                            <td height=\"24\" style=\"height:24px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            //     html.Append("                        </tr>");
            //     // Activate timer when implementation is working 
            //     html.Append("                        <tr>");
            //     html.Append(
            //         "                            <td align=\"center\" valign=\"top\"><img src=\"https://img1.niftyimages.com/spc/t92i/1dc5\" width=\"187\" alt=\"Countdown timer to Jul 13, 2024, 11:59PM AEST\" border=\"0\" style=\"display: block; max-width: 187px; font-family: Arial,sans-serif; font-size: 15px; line-height: 17px; color: #000000;\"/></td>");
            //     html.Append("                        </tr>");
            //     html.Append("                        <tr>");
            //     html.Append(
            //         "                             <td height=\"19\" style=\"height: 19px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            //     html.Append("                        </tr>");
            //     html.Append("                     </tbody>");
            //     html.Append("                     </table>"); //no match
            //     html.Append("                   </td>");
            //     html.Append("               </tr>");
            // }

            html.Append("                   <tr>");


            if (tour.TourSummaryInfo.Images.Any())
            {
                int blendWidth = 0;
                string logoImage;
                (logoImage, blendWidth) = GetLogoImage2024(tour);
                string primaryImage =
                    //$"{SiteContext.CurrentSite.SitePresentationURL}/getmedia/ca9ae809-3de2-4f45-8864-91400205f49c/SPAIN_cordoba-bridge-sunset.jpg?auto=format".Imgixify();
                    tour.TourSummaryInfo.Images.FirstOrDefault().ImagePath.RemoveTilde().Imgixify(540, 0);
                if (!logoImage.IsNullOrEmpty())
                {
                    primaryImage =
                        $"{primaryImage}&blend-mode=normal&blend-align=top,left&blend-w={blendWidth}&blend-pad=20&blend={logoImage.ImgixifyNoParameter()}";
                }

                html.Append(
                    $"                  <td align=\"center\" valign=\"top\" class=\"em_full_img\"><img src=\"{primaryImage}\" width=\"540\" alt=\"\" border=\"0\" style=\"display: block; max-width: 540px;\"/></td>");
            }

            html.Append("                    </tr>");
            html.Append("                    <tr>");
            html.Append(
                "                       <td align=\"center\" valign=\"top\" bgcolor=\"#ffffff\" style=\"border-left: 1px solid #E4E4E4; border-right:1px solid #E4E4E4; padding: 0px 20px; \"><table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                          <tbody>");
            html.Append("                             <tr>");
            html.Append(
                "                                <td height=\"20\" style=\"height: 20px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            html.Append("                             </tr>");

            ///// featured
            html.Append("                             <tr>");
            html.Append(
                "                                <td align=\"center\" valign=\"top\" style=\"padding-bottom: 6px;\">");
            html.Append(
                "                                   <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" dir=\"rtl\">");
            html.Append("                                   <tbody>");
            html.Append("                                      <tr>");
            html.Append(
                "                                         <th align=\"center\" valign=\"top\" class=\"em_clear\">");
            html.Append(
                "                                           <table width=\"151\" align=\"center\" bgcolor=\"#333333\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color:#333333; border-radius:30px; width: 151px;\" class=\"em_wrapper\" dir=\"ltr\">");
            html.Append("                                             <tr>");
            html.Append("                                               <td align=\"center\" valign=\"top\">");
            html.Append(
                "                                                 <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                                                  <tbody>");
            html.Append("                                                     <tr>");
            if (tour.TourSummaryInfo.Promotion != null)
            {
                html.Append(
                    $"                                                  <td class=\"em_defaultlink\" align=\"center\" valign=\"middle\" height=\"21\" style=\"font-size: 12px; font-family: Arial, sans-serif; font-weight:bold; color: #ffffff; height:21px; text-transform: uppercase; letter-spacing: 0.8px;\" ><a href=\"#\" target=\"_blank\" style=\"text-decoration:none; color:#ffffff; line-height:21px; display:block;\">{tour.TourSummaryInfo.Promotion.PromotionName}</a></td>");
            }

            html.Append("                                                     </tr>");
            html.Append("                                                   </tbody>");
            html.Append("                                                   </table>");
            html.Append("                                                </td>");
            html.Append("                                            </tr>");
            html.Append("                                            </table>");
            html.Append("                                         </th>");
            html.Append(
                "                                         <th align=\"left\" valign=\"top\" class=\"em_clear\">");
            html.Append(
                "                                           <table width=\"347\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"width: 347px;\" class=\"em_wrapper\" dir=\"ltr\">");
            html.Append("                                              <tbody>");
            html.Append("                                                <tr>");
            html.Append(
                "                                                  <td align=\"center\" valign=\"top\"><table width=\"100%\" align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                                                      <tbody>");
            html.Append("                                                        <tr>");
            html.Append(
                "                                                          <td align=\"center\" valign=\"top\" class=\"em_ptop\">");
            html.Append(
                "                                                             <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">");
            html.Append("                                                              <tbody>");
            html.Append("                                                                <tr>");
            html.Append(
                $"                                                                  <td align=\"left\" valign=\"middle\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 12px; text-transform: uppercase; line-height: 24px; color: #333333; font-weight: 400; letter-spacing: 0.1px;\"><span style=\"font-weight: bold; \">{tour.TourSummaryInfo.PrimaryCountryName}</span> <span style=\"color: #929497;\">(Package Code: {tour.TourSummaryInfo.TourCode})</span></td>");
            html.Append("                                                                </tr>");
            html.Append("                                                               </tbody>");
            html.Append("                                                              </table>");
            html.Append("                                                           </td>");
            html.Append(
                "                                                           <td width=\"69\" style=\"width: 69px;\" class=\"em_hide\">&nbsp;</td>");
            html.Append("                                                         </tr>");
            html.Append("                                                       </tbody>");
            html.Append("                                                     </table>");
            html.Append("                                                    </td>");
            html.Append("                                                   </tr>");
            html.Append("                                                 </tbody>");
            html.Append("                                             </table>");
            html.Append("                                          </th>");
            html.Append("                                    </tr>");
            html.Append("                                 </tbody>");
            html.Append("                                 </table>");
            html.Append("                              </td>");
            html.Append("                             </tr>");


            ///// end featured
            html.Append("                             <tr>");
            html.Append(
                $"                                 <td align=\"left\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 28px; line-height: 38px; font-weight: bold; color: #CA568E; padding-bottom:5px;\">{tour.TourSummaryInfo.Name}</td>");
            html.Append("                             </tr>");
            html.Append("                             <tr>");
            html.Append(
                $"                                 <td align=\"left\" valign=\"middle\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 12px; text-transform: uppercase; line-height: 24px; color: #333333; padding-bottom: 22px; font-weight: 400; letter-spacing: 0.1px;\">Starts {(tour.TourSummaryInfo.TourDepartsFromInAustralia == tour.TourSummaryInfo.TourTravelEnds ? null : $"from <strong>{tour.TourSummaryInfo.TourDepartsFromInAustralia}</strong>")} and ends in <strong>{tour.TourSummaryInfo.TourTravelEnds}</strong></td>");
            html.Append("                             </tr>");
            html.Append("                             <tr>");
            html.Append(
                $"                                 <td align=\"left\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 18px; line-height: 26px; color: #333333; padding-bottom: 20px;\">{@tour.TourSummaryInfo.Summary}</td>");
            html.Append("                             </tr>");
            html.Append("                             <tr>");
            html.Append("                                  <td align=\"left\" valign=\"top\">");
            html.Append(
                "                                     <table align=\"left\" bgcolor=\"#CA568E\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color:#CA568E; border-radius:30px;\">");
            html.Append("                                         <tr>");
            html.Append(
                $"                                          <td class=\"em_defaultlink\" align=\"center\" valign=\"middle\" height=\"48\" style=\"font-size: 14px; font-family: Arial, sans-serif; font-weight:bold; color: #ffffff; height:48px; text-transform: uppercase; padding: 0 20px;\" ><a href=\"{tour.TourSummaryInfo.Path.ToLower()}\" target=\"_blank\" style=\"text-decoration:none; color:#ffffff; line-height:48px; display:block;\">{ctaLabel}</a></td>");
            html.Append("                                         </tr>");
            html.Append("                                     </table>");
            html.Append("                                   </td>");
            html.Append("                             </tr>");
            html.Append("                             <tr>");
            html.Append(
                "                                  <td height=\"40\" style=\"height: 40px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            html.Append("                             </tr>");
            html.Append("                    </tbody>");
            html.Append("            </table>");
            html.Append("          </td>");
            html.Append("        </tr>");
            html.Append("        <tr>");
            html.Append(
                "            <td align=\"center\" valign=\"top\"><table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >");
            html.Append("                <tbody>");
            html.Append("                   <tr>");
            html.Append(
                $"                    <td align=\"left\" valign=\"top\" bgcolor=\"{(isFeatured ? "#333333" : "#ededed")}\" style=\"border-bottom-left-radius: 8px;\">");
            html.Append(
                "                       <table align=\"left\" width=\"270\" style=\"width: 270px;\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                         <tr>");
            html.Append("                           <td align=\"center\" valign=\"top\" >");
            html.Append(
                "                               <table align=\"center\" style=\"width: 270px;\" class=\"em_widthB\" width=\"270\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            html.Append("                                    <tbody>"); //this 
            html.Append("                                       <tr>");
            html.Append(
                $"                                        <td align=\"center\" valign=\"top\" bgcolor={(isFeatured ? "\"#333333\"" : "\"#ededed\"")} class=\"em_aside20\" style=\"padding: 0px 20px; border-bottom-left-radius: 8px;\" >");
            html.Append(
                "                                            <table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">");
            html.Append("                                               <tbody>");
            html.Append("                                                   <tr>");
            html.Append(
                "                                                      <td height=\"20\" style=\"height:20px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            html.Append("                                                   </tr>");
            html.Append("                                                   <tr>");
            html.Append(
                $"                                                      <td align=\"left\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 20px; line-height:25px; font-weight: bold; color: {(isFeatured ? "#ffffff" : "#333333")}; padding-bottom:12px;\">{tour.TourSummaryInfo.NoOfNights + 1} days</td>");
            html.Append("                                                   </tr>");

            if (tour.TourSummaryInfo.PriceInclusionIcons.Any())
            {
                html.Append("                                               <tr>");
                html.Append("                                                   <td align=\"left\" valign=\"top\">");

                #region Table

                html.Append(
                    "                                                       <table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                html.Append("                                                          <tbody>");
                html.Append("                                                               <tr>");
                if (tour.TourSummaryInfo.PriceInclusionIcons.Exists(x => x.Name == "Flights"))
                {
                    html.Append(
                        $"                                                              <td align=\"center\" valign=\"middle\" style=\"line-height: 0px; font-size: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/{(isFeatured ? "flight-white.png" : "flight-charcoal.png")}\" width=\"20\" alt=\"\" border=\"0\" style=\"display: block; max-width: 20px;\"/></td>");
                    html.Append(
                        "                                                               <td width=\"22\" style=\"width: 22px; line-height: 0px; font-size: 0px;\"></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusionIcons.Exists(x => x.Name == "Accommodation"))
                {
                    html.Append(
                        $"                                                              <td align=\"center\" valign=\"middle\" style=\"line-height: 0px; font-size: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/{(isFeatured ? "img3.png" : "img6.png")}\" width=\"20\" alt=\"\" border=\"0\" style=\"display: block; max-width: 20px;\"/></td>");
                    html.Append(
                        "                                                               <td width=\"22\" style=\"width: 22px; line-height: 0px; font-size: 0px;\"></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusionIcons.Exists(x => x.Name == "Meals"))
                {
                    html.Append(
                        $"                                                              <td align=\"center\" valign=\"top\" style=\"line-height: 0px; font-size: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/{(isFeatured ? "img4.png" : "img7.png")}\" width=\"17\" alt=\"\" border=\"0\" style=\"display: block; max-width:17px;\"/></td>");
                    html.Append(
                        "                                                               <td width=\"24\" style=\"width: 22px; line-height: 0px; font-size: 0px;\"></td>");
                }

                if (tour.TourSummaryInfo.PriceInclusionIcons.Exists(x => x.Name == "Transfer"))
                {
                    html.Append(
                        $"                                                              <td align=\"center\" valign=\"top\" style=\"line-height: 0px; font-size: 0px;\"><img src=\"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/{(isFeatured ? "img5.png" : "img8.png")}\" width=\"17\" alt=\"\" border=\"0\" style=\"display: block; max-width:17px;\"/>");
                    html.Append("                                                               </td>");
                }

                html.Append("                                                               </tr>");
                html.Append("                                                           </tbody>");
                html.Append("                                                       </table>");

                #endregion

                html.Append("                                                   </td>");
                html.Append("                                               </tr>");
            }

            if (!string.IsNullOrWhiteSpace(savingsMessage) ||
                (tour.TourSummaryInfo.FromPrice != null && tour.TourSummaryInfo.Promotion != null))
            {
                html.Append("                                              <tr>");
                html.Append(
                    "                                                  <td align=\"left\" valign=\"top\" style=\"padding-top:10px;\">");
                html.Append(
                    "                                                       <table align=\"left\" bgcolor=\"#CA568E\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color:#CA568E; border-radius:30px; padding: 5px 20px;\">");
                html.Append("                                                           <tbody>");
                html.Append("                                                               <tr>");
                if (!string.IsNullOrWhiteSpace(savingsMessage))
                {
                    html.Append(
                        $"                                                                  <td class=\"em_defaultlink\" valign=\"middle\" height=\"21\" align=\"center\" style=\"font-size: 12px; font-family: Arial, sans-serif; font-weight:bold; color: #ffffff; height:21px; text-transform: uppercase;\">{savingsMessage}");
                    html.Append("                                                                    </td>");
                }
                else
                {
                    html.Append(
                        $"                                                                  <td class=\"em_defaultlink\" valign=\"middle\" height=\"21\" align=\"center\" style=\"font-size: 12px; font-family: Arial, sans-serif; font-weight:bold; color: #ffffff; height:21px; text-transform: uppercase;\"> Save {tour.TourSummaryInfo.Promotion.GetDiscount(tour.TourSummaryInfo.FromPrice.LowestPrice, 1)}");
                    html.Append("                                                                    </td>");
                }

                html.Append("                                                               </tr>");
                html.Append("                                                            </tbody>");
                html.Append("                                                       </table>");
                html.Append("                                                   </td>");
                html.Append("                                               </tr>");
            }

            html.Append("                                                   <tr>");
            html.Append(
                $"                                                       <td height=\"{(tour.TourSummaryInfo.FromPrice != null && tour.TourSummaryInfo.Promotion != null ? "15" : "32")}\" style=\"height:{(tour.TourSummaryInfo.FromPrice != null && tour.TourSummaryInfo.Promotion != null ? "15" : "32")}px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            html.Append("                                                   </tr>");
            html.Append("                                               </tbody>");
            html.Append("                                           </table>");
            html.Append("                                          </td>");
            html.Append("                                        </tr>");
            html.Append("                                       </tbody>"); //this
            html.Append("                           </table>");
            html.Append("                          </td>");
            html.Append("                         </tr>");
            html.Append("                        </table>");
            html.Append("                  </td>");
            if (tour.TourSummaryInfo.FromPrice != null)
            {
                html.Append(
                    "              <td align=\"right\" valign=\"top\" style=\"border-bottom-right-radius: 8px;\" bgcolor=\"#CA568E\">");
                html.Append(
                    "                   <table align=\"right\" width=\"270\" style=\"width: 270px;\" class=\"em_wrapper\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                html.Append("                      <tr>");
                html.Append(
                    "                         <td align=\"center\" valign=\"top\" style=\"border-bottom-right-radius: 8px;\" bgcolor=\"#CA568E\"  >");
                html.Append(
                    "                           <table align=\"center\" style=\"width: 270px;\" class=\"em_widthA\" width=\"270\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                html.Append("                             <tbody>");
                html.Append("                                 <tr>");
                html.Append(
                    "                                   <td align=\"center\" valign=\"top\" bgcolor=\"#CA568E\"   class=\"em_aside20\" style=\"padding: 0px 20px; border-bottom-right-radius: 8px;\" >");
                html.Append(
                    "                                       <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" bgcolor=\"#CA568E\">");
                html.Append("                                           <tbody>");
                html.Append("                                              <tr>");
                html.Append(
                    "                                                <td height=\"25\" style=\"height:25px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
                html.Append("                                              </tr>");
                html.Append("                                              <tr>");
                if (tour.TourSummaryInfo.FromPrice != null && tour.TourSummaryInfo.Promotion != null)
                {
                    html.Append(
                        $"                                                        <td align=\"right\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 14px;  line-height: 20px; color: #ffffff; font-weight: 400;\">From&nbsp;&nbsp;<span style=\"text-decoration: line-through;\">AUD{tour.TourSummaryInfo.FromPrice.LowestPrice.ToString("#,###")}</span></td>");
                }
                else
                {
                    if (tour.TourSummaryInfo.FromPrice != null)
                    {
                        html.Append(
                            $"                                                        <td align=\"right\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 14px;  line-height: 20px; color: #ffffff; font-weight: 400;\">From</td>");
                    }
                }

                html.Append("                                             </tr>");
                html.Append("                                             <tr>");
                html.Append(
                    tour.TourSummaryInfo.Promotion == null
                        ? $"                                                  <td align=\"right\" valign=\"top\" class=\"em_defaultlink em_f26\" style=\"font-family: Arial,sans-serif; font-size: 30px; line-height: 32px; color: #ffffff; font-weight: 800;\"><sup style=\"font-weight: bold; font-size: 13px; line-height: 15px;\">AUD</sup> <strong>{tour?.TourSummaryInfo.FromPrice.LowestPrice.ToString("#,###")}</strong></td>"
                        : $"                                                  <td align=\"right\" valign=\"top\" class=\"em_defaultlink em_f26\" style=\"font-family: Arial,sans-serif; font-size: 30px; line-height: 32px; color: #ffffff; font-weight: 800;\"><sup style=\"font-weight: bold; font-size: 13px; line-height: 15px;\">AUD</sup> <strong>{tour.TourSummaryInfo.Promotion.GetDiscountedPrice(tour.TourSummaryInfo.FromPrice.LowestPrice).ToString("#,###")}</strong></td>");

                html.Append("                                             </tr>");
                html.Append("                                             <tr>");
                html.Append(
                    $"                                                <td align=\"right\" valign=\"top\" class=\"em_defaultlink\" style=\"font-family: Arial,sans-serif; font-size: 12px;  line-height: 14px; color: #ffffff; font-weight: 400;\">{tour.TourSummaryInfo.PriceTypeLabel}</td>");
                html.Append("                                             </tr>");
                html.Append("                                             <tr>");
                html.Append(
                    "                                                 <td height=\"27\" style=\"height:27px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
                html.Append("                                             </tr>");
                html.Append("                                           </tbody>");
                html.Append("                                       </table>");
                html.Append("                                   </td>");
                html.Append("                                 </tr>");
                html.Append("                             </tbody>");
                html.Append("                           </table>");
                html.Append("                         </td>");
                html.Append("                      </tr>");
                html.Append("                   </table>");
                html.Append("              </td>");
            }


            html.Append("               </tr>");
            html.Append("              </tbody>");
            html.Append("             </table>");
            html.Append("            </td>");
            html.Append("           </tr>");
            html.Append("           <tr>");
            html.Append(
                $"            <td height=\"{(isFeatured ? 30 : 80)}\" style=\"height: {(isFeatured ? 30 : 80)}px; line-height: 0px; font-size: 0px;\">&nbsp;</td>");
            html.Append("           </tr>");
            html.Append("         </tbody>");
            html.Append("        </table>");
            html.Append("       </td>");
            html.Append("      </tr>");
            html.Append("     </tbody>");
            html.Append("    </table>");
            html.Append("   </td>");
            html.Append("  </tr>");
            ///////////////////////////////////////////////


            return html.ToString();
        }

        private static (string, int) GetLogoImage2024(TourModel tour)
        {
            var logoImage = "";
            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourIsExclusive && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Exclusive-Safe-FOC.png";
                return (logoImage, 200);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Exclusive-Safe.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Safe-FOC.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourIsExclusive &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Exclusive-FOC.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourIsExclusive &&
                tour.TourSummaryInfo.TourHasFreedomOfChoice) //nodata
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/Exclusive-Safe-FOC.png";
                return (logoImage, 150);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-FOC.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourIsExclusive && tour.TourSummaryInfo.TourHasFreedomOfChoice) // nodata
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/Exclusive-FOC.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourHasFreedomOfChoice)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/Safe-FOC.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourHasSafeTravel)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Safe.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee && tour.TourSummaryInfo.TourIsExclusive) // nodata
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/POM-Exclusive.png";
                return (logoImage, 100);
            }

            if (tour.TourSummaryInfo.TourHasSafeTravel && tour.TourSummaryInfo.TourIsExclusive)
            {
                logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/Exclusive-Safe.png";
                return (logoImage, 100);
            }

            // if (tour.TourSummaryInfo.HasPeaceOfMindGuarantee)
            // {
            //     logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/PDF-POM.png";
            //     return (logoImage, 90);
            // }
            //
            // if (tour.TourSummaryInfo.TourHasSafeTravel)
            // {
            //     logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/pdf-safe-travels.png";
            //
            //     return (logoImage, 90);
            // }
            //
            // if (tour.TourSummaryInfo.TourIsExclusive)
            // {
            //     logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/pdf-Exclusive-Packages.png";
            //     return (logoImage, 90);
            // }
            //
            // if (tour.TourSummaryInfo.TourHasFreedomOfChoice)
            // {
            //     logoImage = $"{SiteContext.CurrentSite.SitePresentationURL}/edm/images2024/pdf-freedom-of-choice.png";
            //     return (logoImage, 90);
            // }

            return (string.Empty, 0);
        }
    }
}