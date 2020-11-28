alter table "SECURITY_GROUP_HIERARCHY_NODE"
	add constraint "SECURITY_GROUP_HN_PK" primary key clustered ("HN_RID", "GROUP_RID", "ACTION_ID", "SEC_TYPE")   



GO
