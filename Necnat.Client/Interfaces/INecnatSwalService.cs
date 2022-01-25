using CurrieTechnologies.Razor.SweetAlert2;

namespace Necnat.Client.Interfaces
{
    public interface INecnatSwalService
    {
        public SweetAlertOptions Success();
        public SweetAlertOptions Error();
        public SweetAlertOptions Warning();
        public SweetAlertOptions Info();
        public SweetAlertOptions Question();
        public SweetAlertOptions Canceled();

        public SweetAlertOptions Success(string text);
        public SweetAlertOptions Error(string text);
        public SweetAlertOptions Warning(string text);
        public SweetAlertOptions Info(string text);
        public SweetAlertOptions Question(string text);
        public SweetAlertOptions Canceled(string text);

        public SweetAlertOptions ErrorGeneric();
        public SweetAlertOptions WarningConfirmAction(PreConfirmCallback preConfirmCallback);
        public SweetAlertOptions WarningConfirmActionCanceled();
        public SweetAlertOptions WarningInvalidFields();
    }
}
