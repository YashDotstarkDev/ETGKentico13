using CMS.CustomTables;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using ETG.Core.CustomTables;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Forms
{
    public class BizformEntry<T> : IBizformEntry<T> where T : BizFormItem
    {
        AbstractValidator<T> _validator;
        protected T _bizformItem;
        BizFormMailSender _mailSender;
        public string OverrideNotificationEmail { get; set; }
        public void Initialize(AbstractValidator<T> validator, T bizformItem)
        {
            _validator = validator;
            _bizformItem = bizformItem;

            var formInfo = FormHelper.GetFormInfo(bizformItem.ClassName, true);
            _mailSender = new BizFormMailSender(bizformItem.BizFormInfo, bizformItem, formInfo);
        }

        public bool Validate()
        {
            if (_validator == null)
            {
                throw new Exception("Validator not initialized, call Initialize before using this method");
            }
            var result = _validator.Validate<T>(_bizformItem);

            return result.IsValid;
        }

        public T Save()
        {
            if (_bizformItem == null)
            {
                throw new Exception("bizformItem is null, call Initialize before using this method");
            }

            var firstName = ValidationHelper.GetString(_bizformItem.GetValue("FirstName"), string.Empty);
            var lastName = ValidationHelper.GetString(_bizformItem.GetValue("LastName"), string.Empty);
            var email = ValidationHelper.GetString(_bizformItem.GetValue("Email"), string.Empty);
            var exchangeItem = CustomTableItemProvider.GetItems<RestrictedDomainItem>().TypedResult;

            int firstNameVowelCount = 0, lastNameVowelCount = 0;
            var vowelList = new List<string>() {
                "a", "e", "i", "o", "u", "y"
            };

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(email))
            {
                foreach (string vowel in vowelList)
                {
                    if (firstName.ToLower().IndexOf(vowel) > -1)
                    {
                        firstNameVowelCount += 1;
                    }

                    if (lastName.ToLower().IndexOf(vowel) > -1)
                    {
                        lastNameVowelCount += 1;
                    }
                }

                if (firstNameVowelCount < 1 || lastNameVowelCount < 1)
                {
                    _bizformItem.SetValue("IsSpam", true);
                }
                else
                {
                    if (exchangeItem != null)
                    {
                        foreach (var item in exchangeItem)
                        {
                            if (email.ToLower().Contains(item.DomainName))
                            {
                                _bizformItem.SetValue("IsSpam", true);
                                break;
                            }
                        }
                    }
                }
            }

            BizFormItemProvider.SetItem(_bizformItem);
            return _bizformItem;
        }

        public virtual void SendNotificationEmail()
        {
            if (_bizformItem == null || _mailSender == null)
            {
                return;
            }

            _mailSender.SendNotificationEmail();

        }

        public void SendConfirmationEmail()
        {
            if (_bizformItem == null || _mailSender == null)
            {
                return;
            }

            _mailSender.SendConfirmationEmail();
        }

        public string GetRedirectUrl()
        {
            if (_bizformItem == null)
            {
                return string.Empty;
            }

            return _bizformItem.BizFormInfo.FormRedirectToUrl;
        }

        public bool Submit()
        {
            var newItem = Save();

            SendNotificationEmail();
            SendConfirmationEmail();

            if (newItem.ItemID > 0)
            {
                return true;
            }

            return false;
        }
    }
}