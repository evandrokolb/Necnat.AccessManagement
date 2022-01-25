using Microsoft.Extensions.Localization;
using Necnat.Shared.Interfaces;
using Necnat.Shared.Models;
using Necnat.Shared.Resources.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Necnat.Shared.Validators
{
    public class StringValidator : IValidator
    {
        private readonly IStringLocalizer<CommonValidatorLocalizer> _commonValidatorLocalizer;
        private readonly IStringLocalizer<StringValidatorLocalizer> _stringValidatorLocalizer;

        public StringValidator(IStringLocalizer<CommonValidatorLocalizer> commonValidatorLocalizer, 
            IStringLocalizer<StringValidatorLocalizer> stringValidatorLocalizer) 
        {
            _commonValidatorLocalizer = commonValidatorLocalizer;
            _stringValidatorLocalizer = stringValidatorLocalizer;
        }

        public MdValidatorResult Required(string s, string fieldName)
        {
            var r = new MdValidatorResult();
            r.IsValid = !string.IsNullOrWhiteSpace(s);

            if (!r.IsValid)
                r.Message = string.Format(_commonValidatorLocalizer["FIELD_REQUIRED"].Value, fieldName);

            return r;
        }

        public MdValidatorResult ExactLength(string s, string fieldName, int length)
        {
            var r = new MdValidatorResult();
            r.IsValid = s != null && s.Length == length;

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["EXACT_LENGTH"].Value, fieldName, length);

            return r;
        }

        public MdValidatorResult MinLength(string s, string fieldName, int minLength)
        {
            var r = new MdValidatorResult();
            r.IsValid = s != null && s.Length >= minLength;

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["MIN_LENGTH"].Value, fieldName, minLength);

            return r;
        }

        public MdValidatorResult MaxLength(string s, string fieldName, int maxLength)
        {
            var r = new MdValidatorResult();
            r.IsValid = string.IsNullOrWhiteSpace(s) ? true : s.Length <= maxLength;

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["MAX_LENGTH"].Value, fieldName, maxLength);

            return r;
        }

        public MdValidatorResult MinMaxLength(string s, string fieldName, int minLength, int maxLength)
        {
            var r = new MdValidatorResult();
            r.IsValid = s != null && s.Length >= minLength && s.Length <= maxLength;

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["MIN_MAX_LENGTH"].Value, fieldName, minLength, maxLength);

            return r;
        }

        public MdValidatorResult FromDataAnnotation(string s, string fieldName, Type type, string propertyName)
        {
            if (s == null)
                s = string.Empty;

            var r = new MdValidatorResult();
            r.IsValid = true;

            //RequiredAttribute
            var requiredAttribute = type.GetProperties()
                .Where(p => p.Name == propertyName)
                .Single()
                .GetCustomAttributes(typeof(RequiredAttribute), true)
                .FirstOrDefault() as RequiredAttribute;

            if (requiredAttribute != null)
            {
                r.IsValid = !string.IsNullOrWhiteSpace(s);

                if (!r.IsValid)
                {
                    r.Message = string.Format(_commonValidatorLocalizer["FIELD_REQUIRED"].Value, fieldName);
                    return r;
                }
            }

            //StringLengthAttribute
            var stringLengthAttribute = type.GetProperties()
            .Where(p => p.Name == propertyName)
            .Single()
            .GetCustomAttributes(typeof(StringLengthAttribute), true)
            .FirstOrDefault() as StringLengthAttribute;

            if (stringLengthAttribute != null)
            {
                if (stringLengthAttribute.MinimumLength > 0)
                    r.IsValid = s.Length >= stringLengthAttribute.MinimumLength;

                if (!r.IsValid)
                {
                    r.Message = string.Format(_stringValidatorLocalizer["MIN_LENGTH"].Value, fieldName, stringLengthAttribute.MinimumLength);
                    return r;
                }

                if (stringLengthAttribute.MaximumLength > 0)
                    r.IsValid = s.Length <= stringLengthAttribute.MaximumLength;

                if (!r.IsValid)
                {
                    r.Message = string.Format(_stringValidatorLocalizer["MAX_LENGTH"].Value, fieldName, stringLengthAttribute.MaximumLength);
                    return r;
                }
            }

            return r;
        }

        public MdValidatorResult Alphanumeric(string s, string fieldName)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9]*$");

            var r = new MdValidatorResult();
            r.IsValid = s != null && rgx.IsMatch(s);

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["ALPHANUMERIC"].Value, fieldName);

            return r;
        }

        public MdValidatorResult Regex(string s, string fieldName, string regex, string regexText)
        {
            Regex rgx = new Regex(regex);

            var r = new MdValidatorResult();
            r.IsValid = s != null && rgx.IsMatch(s);

            if (!r.IsValid)
                r.Message = string.Format(_stringValidatorLocalizer["REGEX"].Value, fieldName, regexText);

            return r;
        }
    }
}
