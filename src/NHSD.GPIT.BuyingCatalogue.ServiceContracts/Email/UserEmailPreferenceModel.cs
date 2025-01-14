﻿using NHSD.GPIT.BuyingCatalogue.EntityFramework.Notifications.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Email;

public class UserEmailPreferenceModel
{
    public UserEmailPreferenceModel()
    {
    }

    public UserEmailPreferenceModel(
        EmailPreferenceTypeEnum emailPreferenceType,
        bool defaultEnabled,
        bool? userEnabled,
        EmailPreferenceRoleType roleType)
    {
        EmailPreferenceType = emailPreferenceType;
        DefaultEnabled = defaultEnabled;
        UserEnabled = userEnabled;
        Enabled = UserEnabled ?? DefaultEnabled;
        RoleType = roleType;
    }

    public EmailPreferenceTypeEnum EmailPreferenceType { get; set; }

    public bool DefaultEnabled { get; set; }

    public bool? UserEnabled { get; set; }

    public bool Enabled { get; set; }

    public EmailPreferenceRoleType RoleType { get; set; }
}
