/* Add foreign key constraints to table "SECURITY_FUNCTION_ACTIONS".                          */
alter table "SECURITY_FUNCTION_ACTIONS"
	add constraint "SEC_FUNC_JOIN_SEC_FUNC_ACT_FK1" foreign key (
		"FUNC_ID")
	 references "SECURITY_FUNCTION_JOIN" (
		"FUNC_ID")  

go
