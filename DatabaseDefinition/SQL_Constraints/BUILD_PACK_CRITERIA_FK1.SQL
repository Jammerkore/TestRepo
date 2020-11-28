/* Begin MOD - JScott - Build Pack Criteria Load */
alter table "BUILD_PACK_COMBO"
	add constraint "BUILD_PACK_CRITERIA_FK1" foreign key (
		"BPC_RID")
	 references "BUILD_PACK_CRITERIA" (
		"BPC_RID")  

GO
