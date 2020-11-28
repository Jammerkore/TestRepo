alter table "SECURITY_USER_HIERARCHY_NODE"
	add constraint "SECURITY_USER_HN_PK" primary key clustered ("HN_RID", "USER_RID", "ACTION_ID", "SEC_TYPE")   



GO
