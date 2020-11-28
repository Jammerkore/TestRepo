/* Add foreign key constraints to table "USER_PLAN_BASIS_DETAILS".                            */
alter table "USER_PLAN_BASIS_DETAILS"
	add constraint "USER_PSB_Table1_FK1" foreign key (
		"USER_RID",
		"BASIS_RID")
	 references "USER_PLAN_BASIS" (
		"USER_RID",
		"BASIS_RID")  

go
