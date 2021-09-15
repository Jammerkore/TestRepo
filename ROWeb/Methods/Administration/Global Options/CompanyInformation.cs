using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetCompanyInfo()
        {
            CompanyInfoRowHandler rowHandler = CompanyInfoRowHandler.GetInstance(_GlobalOptionsProfile, _SmtpOptions);
            DataTable dt = BuildCompanyDataTable(rowHandler);

            if (_GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddCompanyInfo(dt, rowHandler);
            }

            return dt;
        }

        private DataTable BuildCompanyDataTable(CompanyInfoRowHandler rowHandler)
        {
            DataTable dt = new DataTable("Company Information");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }

        private void AddCompanyInfo(DataTable dt, CompanyInfoRowHandler rowHandler)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);
        }

        private void UpdateCompanyInformation(DataTable dtCompanyInformation)
        {
            DataRow dr = dtCompanyInformation.Rows[0];

            CompanyInfoRowHandler rowHandler = CompanyInfoRowHandler.GetInstance(_GlobalOptionsProfile, _SmtpOptions);

            rowHandler.ParseUIRow(dr);
        }

        public class CompanyInfoRowHandler : RowHandler
        {
            private static CompanyInfoRowHandler _Instance;

            public static CompanyInfoRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile, GlobalOptions_SMTP_BL SmtpOptions)
            {
                if (_Instance == null)
                {
                    _Instance = new CompanyInfoRowHandler(GlobalOptionsProfile, SmtpOptions);
                }
                else
                {
                    _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
                    _Instance._SmtpOptions = SmtpOptions;
                }

                return _Instance;
            }

            private GlobalOptionsProfile _GlobalOptionsProfile;
            private GlobalOptions_SMTP_BL _SmtpOptions;


            private TypedColumnHandler<string> _CompanyNameColHandler;
            private TypedColumnHandler<string> _PhoneNumberColHandler;
            private TypedColumnHandler<string> _FaxNumberColHandler;
            private TypedColumnHandler<string> _StreetAddressColHandler;
            private TypedColumnHandler<string> _CityColHandler;
            private TypedColumnHandler<string> _StateColHandler;
            private TypedColumnHandler<string> _ZipCodeColHandler;
            private TypedColumnHandler<string> _ProductLevelDelimColHandler;
            private TypedColumnHandler<string> _RecipientEmailAddressColHandler;
            private TypedColumnHandler<bool> _SMTPEnabledColHandler;
            private TypedColumnHandler<string> _SMTPFromAddressColHandler;
            private TypedColumnHandler<string> _SMTPServerColHandler;
            private TypedColumnHandler<int> _SMTPPortColHandler;
            private TypedColumnHandler<bool> _SMTPUseSSLColHandler;
            private TypedColumnHandler<bool> _SMTPUseDefaultCredentialsColHandler;
            private TypedColumnHandler<string> _SMTPUsernameColHandler;
            private TypedColumnHandler<string> _SMTPPasswordColHandler;
            private TypedColumnHandler<bool> _SMTPUseOutlookContactsColHandler;
            private TypedColumnHandler<bool> _SMTPMessageFormatInHTMLColHandler;
            private TypedColumnHandler<bool> _useExternalEligibilityAllocationHandler;
            private TypedColumnHandler<bool> _useExternalEligibilityPlanningHandler;
            private TypedColumnHandler<int> _externalEligibilityProductIdentifierHandler;
            private TypedColumnHandler<int> _externalEligibilityChannelIdentifierHandler;
            private TypedColumnHandler<string> _externalEligibilityURLHandler;


            protected CompanyInfoRowHandler(GlobalOptionsProfile GlobalOptionsProfile, GlobalOptions_SMTP_BL SmtpOptions)
            {
                _GlobalOptionsProfile = GlobalOptionsProfile;
                _SmtpOptions = SmtpOptions;
                _CompanyNameColHandler =
                    new TypedColumnHandler<string>("Company Name", eMIDTextCode.Unassigned, false, "");
                _PhoneNumberColHandler =
                    new TypedColumnHandler<string>("Phone", eMIDTextCode.Unassigned, false, "");
                _FaxNumberColHandler =
                    new TypedColumnHandler<string>("FAX", eMIDTextCode.Unassigned, false, "");
                _StreetAddressColHandler =
                    new TypedColumnHandler<string>("Street", eMIDTextCode.Unassigned, false, "");
                _CityColHandler =
                    new TypedColumnHandler<string>("City", eMIDTextCode.Unassigned, false, "");
                _StateColHandler =
                    new TypedColumnHandler<string>("State", eMIDTextCode.Unassigned, false, "");
                _ZipCodeColHandler =
                    new TypedColumnHandler<string>("Zip", eMIDTextCode.Unassigned, false, "");
                _ProductLevelDelimColHandler =
                    new TypedColumnHandler<string>("Product Level Delimiter:", eMIDTextCode.Unassigned, false, "");
                _RecipientEmailAddressColHandler =
                    new TypedColumnHandler<string>("Email", eMIDTextCode.Unassigned, false, "");
                _SMTPEnabledColHandler =
                    new TypedColumnHandler<bool>("Enable SMTP Emailing", eMIDTextCode.Unassigned, false, false);
                _SMTPFromAddressColHandler =
                    new TypedColumnHandler<string>("From", eMIDTextCode.Unassigned, false, "");
                _SMTPServerColHandler =
                    new TypedColumnHandler<string>("Server", eMIDTextCode.Unassigned, false, "");
                _SMTPPortColHandler =
                    new TypedColumnHandler<int>("Port", eMIDTextCode.Unassigned, false, 0);
                _SMTPUseSSLColHandler =
                    new TypedColumnHandler<bool>("Enable SMTP Over SSL", eMIDTextCode.Unassigned, false, false);
                _SMTPUseDefaultCredentialsColHandler =
                    new TypedColumnHandler<bool>("Use Default Server Credentials", eMIDTextCode.Unassigned, false, false);
                _SMTPUsernameColHandler =
                    new TypedColumnHandler<string>("User", eMIDTextCode.Unassigned, false, "");
                _SMTPPasswordColHandler =
                    new TypedColumnHandler<string>("Password", eMIDTextCode.Unassigned, false, "");
                _SMTPUseOutlookContactsColHandler =
                    new TypedColumnHandler<bool>("Use Outlook Contact Information", eMIDTextCode.Unassigned, false, false);
                _SMTPMessageFormatInHTMLColHandler =
                    new TypedColumnHandler<bool>("Message Format", eMIDTextCode.Unassigned, false, false);

                _useExternalEligibilityAllocationHandler =
                    new TypedColumnHandler<bool>("Use External Eligibility Allocation", eMIDTextCode.Unassigned, false, false);
                _useExternalEligibilityPlanningHandler =
                    new TypedColumnHandler<bool>("Use External Eligibility Planning", eMIDTextCode.Unassigned, false, false);
                _externalEligibilityProductIdentifierHandler =
                   new TypedColumnHandler<int>("Eligibility Product Identifier", eMIDTextCode.Unassigned, false, 0);
                _externalEligibilityChannelIdentifierHandler =
                   new TypedColumnHandler<int>("Eligibility Channel Identifier", eMIDTextCode.Unassigned, false, 0);
                _externalEligibilityURLHandler =
                   new TypedColumnHandler<string>("Eligibility URL", eMIDTextCode.Unassigned, false, "");

                _aColumnHandlers = new ColumnHandler[] { _CompanyNameColHandler, _PhoneNumberColHandler, _FaxNumberColHandler, _StreetAddressColHandler,
                                                         _CityColHandler, _StateColHandler, _ZipCodeColHandler, _ProductLevelDelimColHandler, _RecipientEmailAddressColHandler,
                                                         _SMTPEnabledColHandler, _SMTPFromAddressColHandler, _SMTPServerColHandler, _SMTPPortColHandler, _SMTPUseSSLColHandler,
                                                         _SMTPUseDefaultCredentialsColHandler, _SMTPUsernameColHandler, _SMTPPasswordColHandler,
                                                         _SMTPUseOutlookContactsColHandler,
                                                         _SMTPMessageFormatInHTMLColHandler,
                                                         _useExternalEligibilityAllocationHandler,
                                                         _useExternalEligibilityPlanningHandler,
                                                         _externalEligibilityProductIdentifierHandler,
                                                         _externalEligibilityChannelIdentifierHandler,
                                                         _externalEligibilityURLHandler
                };
            }

            public override void ParseUIRow(DataRow dr)
            {
                _GlobalOptionsProfile.CompanyName = _CompanyNameColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.Telephone = _PhoneNumberColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.Fax = _FaxNumberColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.Street = _StreetAddressColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.City = _CityColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.State = _StateColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.Zip = _ZipCodeColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.ProductLevelDelimiter = _ProductLevelDelimColHandler.ParseUIColumn(dr)[0];
                _GlobalOptionsProfile.Email = _RecipientEmailAddressColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Enabled.Value = _SMTPEnabledColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_From_Address.Value = _SMTPFromAddressColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Server.Value = _SMTPServerColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Port.Value = _SMTPPortColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Use_SSL.Value = _SMTPUseSSLColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Use_Default_Credentials.Value = _SMTPUseDefaultCredentialsColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_UserName.Value = _SMTPUsernameColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Pwd.Value = _SMTPPasswordColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_Use_Outlook_Contacts.Value = _SMTPUseOutlookContactsColHandler.ParseUIColumn(dr);
                _SmtpOptions.SMTP_MessageFormatInHTML.Value = _SMTPMessageFormatInHTMLColHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.UseExternalEligibilityAllocation = _useExternalEligibilityAllocationHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.UseExternalEligibilityPlanning = _useExternalEligibilityPlanningHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.ExternalEligibilityProductIdentifier = 
                    (eExternalEligibilityProductIdentifier)_externalEligibilityProductIdentifierHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.ExternalEligibilityChannelIdentifier =
                    (eExternalEligibilityChannelIdentifier)_externalEligibilityChannelIdentifierHandler.ParseUIColumn(dr);
                _GlobalOptionsProfile.ExternalEligibilityURL = _externalEligibilityURLHandler.ParseUIColumn(dr);
            }

            public override void FillUIRow(DataRow dr)
            {
                _CompanyNameColHandler.SetUIColumn(dr, _GlobalOptionsProfile.CompanyName);
                _PhoneNumberColHandler.SetUIColumn(dr, _GlobalOptionsProfile.Telephone);
                _FaxNumberColHandler.SetUIColumn(dr, _GlobalOptionsProfile.Fax);
                _StreetAddressColHandler.SetUIColumn(dr, _GlobalOptionsProfile.Street);
                _CityColHandler.SetUIColumn(dr, _GlobalOptionsProfile.City);
                _StateColHandler.SetUIColumn(dr, _GlobalOptionsProfile.State);
                _ZipCodeColHandler.SetUIColumn(dr, _GlobalOptionsProfile.Zip);
                _ProductLevelDelimColHandler.SetUIColumn(dr, Convert.ToString(_GlobalOptionsProfile.ProductLevelDelimiter));
                _RecipientEmailAddressColHandler.SetUIColumn(dr, _GlobalOptionsProfile.Email);
                _SMTPEnabledColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Enabled.Value);
                _SMTPFromAddressColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_From_Address.Value);
                _SMTPServerColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Server.Value);
                _SMTPPortColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Port.Value);
                _SMTPUseSSLColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Use_SSL.Value);
                _SMTPUseDefaultCredentialsColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Use_Default_Credentials.Value);
                _SMTPUsernameColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_UserName.Value);
                _SMTPPasswordColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Pwd.Value);
                _SMTPUseOutlookContactsColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_Use_Outlook_Contacts.Value);
                _SMTPMessageFormatInHTMLColHandler.SetUIColumn(dr, _SmtpOptions.SMTP_MessageFormatInHTML.Value);
                _useExternalEligibilityAllocationHandler.SetUIColumn(dr, _GlobalOptionsProfile.UseExternalEligibilityAllocation);
                _useExternalEligibilityPlanningHandler.SetUIColumn(dr, _GlobalOptionsProfile.UseExternalEligibilityPlanning);
                int identifierOption = _GlobalOptionsProfile.ExternalEligibilityProductIdentifier.GetHashCode();
                _externalEligibilityProductIdentifierHandler.SetUIColumn(dr, identifierOption);
                identifierOption = _GlobalOptionsProfile.ExternalEligibilityChannelIdentifier.GetHashCode();
                _externalEligibilityChannelIdentifierHandler.SetUIColumn(dr, identifierOption);
                _externalEligibilityURLHandler.SetUIColumn(dr, _GlobalOptionsProfile.ExternalEligibilityURL);
            }
        }
    }
}
