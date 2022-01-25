using Microsoft.Extensions.Localization;
using Necnat.Shared.Interfaces;
using Necnat.Shared.Models;
using Necnat.Shared.Resources.Validators;

namespace Necnat.Shared.Validators
{
    public class IntegerValidator : IValidator
    {
        private readonly IStringLocalizer<CommonValidatorLocalizer> _commonValidatorLocalizer;
        private readonly IStringLocalizer<IntegerValidatorLocalizer> _integerValidatorLocalizer;

        public IntegerValidator(IStringLocalizer<CommonValidatorLocalizer> commonValidatorLocalizer,
            IStringLocalizer<IntegerValidatorLocalizer> integerValidatorLocalizer)
        {
            _commonValidatorLocalizer = commonValidatorLocalizer;
            _integerValidatorLocalizer = integerValidatorLocalizer;
        }

        public MdValidatorResult Required(int? i, string fieldName)
        {
            var r = new MdValidatorResult();
            r.IsValid = i != null;

            if (!r.IsValid)
                r.Message = string.Format(_commonValidatorLocalizer["FIELD_REQUIRED"].Value, fieldName);

            return r;
        }

        public MdValidatorResult NotNullNor0(int? i, string fieldName)
        {
            var r = new MdValidatorResult();
            r.IsValid = i != null && i != 0;

            if (!r.IsValid)
                r.Message = string.Format(_integerValidatorLocalizer["NOT_NULL_NOR_0"].Value, fieldName);

            return r;
        }
    }
}
