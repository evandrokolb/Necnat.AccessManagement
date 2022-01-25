using Microsoft.Extensions.Localization;
using Necnat.Shared.Interfaces;
using Necnat.Shared.Models;
using Necnat.Shared.Resources.Validators;
using System;

namespace Necnat.Shared.Validators
{
    public class DateTimeValidator : IValidator
    {
        private readonly IStringLocalizer<CommonValidatorLocalizer> _commonValidatorLocalizer;
        private readonly IStringLocalizer<DateTimeValidatorLocalizer> _dateTimeValidatorLocalizer;

        public DateTimeValidator(IStringLocalizer<CommonValidatorLocalizer> commonValidatorLocalizer,
            IStringLocalizer<DateTimeValidatorLocalizer> dateTimeValidatorLocalizer)
        {
            _commonValidatorLocalizer = commonValidatorLocalizer;
            _dateTimeValidatorLocalizer = dateTimeValidatorLocalizer;
        }

        public MdValidatorResult Required(DateTime? dt, string fieldName)
        {
            var r = new MdValidatorResult();
            r.IsValid = !(dt == null || dt == new DateTime());

            if (!r.IsValid)
                r.Message = string.Format(_commonValidatorLocalizer["FIELD_REQUIRED"].Value, fieldName);

            return r;
        }

        public MdValidatorResult GreaterDate(DateTime dt, DateTime dtCompare, string fieldName, string msgComplement)
        {
            var r = new MdValidatorResult();
            r.IsValid = dt > dtCompare;

            if (!r.IsValid)
                r.Message = string.Format(_dateTimeValidatorLocalizer["GREATER_DATE"].Value, fieldName, msgComplement);

            return r;
        }

        public MdValidatorResult EqualGreaterDate(DateTime dt, DateTime dtCompare, string fieldName, string msgComplement)
        {
            var r = new MdValidatorResult();
            r.IsValid = dt >= dtCompare;

            if (!r.IsValid)
                r.Message = string.Format(_dateTimeValidatorLocalizer["EQUAL_GREATER_DATE"].Value, fieldName, msgComplement);

            return r;
        }

        public MdValidatorResult EqualDate(DateTime dt, DateTime dtCompare, string fieldName, string msgComplement)
        {
            var r = new MdValidatorResult();
            r.IsValid = dt == dtCompare;

            if (!r.IsValid)
                r.Message = string.Format(_dateTimeValidatorLocalizer["EQUAL_DATE"].Value, fieldName, msgComplement);

            return r;
        }

        public MdValidatorResult EqualLessDate(DateTime dt, DateTime dtCompare, string fieldName, string msgComplement)
        {
            var r = new MdValidatorResult();
            r.IsValid = dt <= dtCompare;

            if (!r.IsValid)
                r.Message = string.Format(_dateTimeValidatorLocalizer["EQUAL_LESS_DATE"].Value, fieldName, msgComplement);

            return r;
        }

        public MdValidatorResult LessDate(DateTime dt, DateTime dtCompare, string fieldName, string msgComplement)
        {
            var r = new MdValidatorResult();
            r.IsValid = dt < dtCompare;

            if (!r.IsValid)
                r.Message = string.Format(_dateTimeValidatorLocalizer["LESS_DATE"].Value, fieldName, msgComplement);

            return r;
        }
    }
}
