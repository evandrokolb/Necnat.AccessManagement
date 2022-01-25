using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.Extensions.Localization;
using Necnat.Client.Interfaces;
using Necnat.Shared.Resources;

namespace Necnat.Client.Services
{
    public class NecnatSwalService : INecnatSwalService
    {
        IStringLocalizer<SwalLocalizer> _swalLocalizer;

        public NecnatSwalService(IStringLocalizer<SwalLocalizer> swalLocalizer)
        {
            _swalLocalizer = swalLocalizer;
        }

        public SweetAlertOptions Success()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Success!"],
                Icon = SweetAlertIcon.Success
            };
        }
        public SweetAlertOptions Error()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Error!"],
                Icon = SweetAlertIcon.Error
            };
        }
        public SweetAlertOptions Warning()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Warning!"],
                Icon = SweetAlertIcon.Warning
            };
        }
        public SweetAlertOptions Info()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Information!"],
                Icon = SweetAlertIcon.Info
            };
        }
        public SweetAlertOptions Question()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Question!"],
                Icon = SweetAlertIcon.Question
            };
        }
        public SweetAlertOptions Canceled()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Canceled!"],
                Icon = SweetAlertIcon.Error
            };
        }

        public SweetAlertOptions Success(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Success!"],
                Text = text,
                Icon = SweetAlertIcon.Success
            };
        }
        public SweetAlertOptions Error(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Error!"],
                Text = text,
                Icon = SweetAlertIcon.Error
            };
        }
        public SweetAlertOptions Warning(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Warning!"],
                Text = text,
                Icon = SweetAlertIcon.Warning
            };
        }
        public SweetAlertOptions Info(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Information!"],
                Text = text,
                Icon = SweetAlertIcon.Info
            };
        }
        public SweetAlertOptions Question(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Question!"],
                Text = text,
                Icon = SweetAlertIcon.Question
            };
        }
        public SweetAlertOptions Canceled(string text)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Canceled!"],
                Text = text,
                Icon = SweetAlertIcon.Error
            };
        }

        public SweetAlertOptions ErrorGeneric()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Error!"],
                Text = _swalLocalizer["An unexpected error has occurred."],
                Icon = SweetAlertIcon.Error
            };
        }
        public SweetAlertOptions WarningConfirmAction(PreConfirmCallback preConfirmCallback)
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Are you sure?"],
                Text = _swalLocalizer["Do you really want to execute this action?"],
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = _swalLocalizer["Yes. Execute"],
                CancelButtonText = _swalLocalizer["No. Cancel"],
                ShowLoaderOnConfirm = true,
                PreConfirm = preConfirmCallback,
                AllowOutsideClick = false
            };
        }
        public SweetAlertOptions WarningConfirmActionCanceled()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Canceled!"],
                Text = _swalLocalizer["Action canceled."],
                Icon = SweetAlertIcon.Error
            };
        }
        public SweetAlertOptions WarningInvalidFields()
        {
            return new SweetAlertOptions
            {
                Title = _swalLocalizer["Warning!"],
                Text = _swalLocalizer["There are invalid fields."],
                Icon = SweetAlertIcon.Warning
            };
        }
    }
}
