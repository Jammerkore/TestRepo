alter table "SECURITY_USER_VERSION"
	add constraint "SECURITY_USER_VERSION_PK" primary key clustered ("USER_RID", "FV_RID", "ACTION_ID", "SEC_TYPE")   



GO
