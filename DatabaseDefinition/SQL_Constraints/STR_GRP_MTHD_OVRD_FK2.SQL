/* BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)  */
alter table "METHOD_OVERRIDE"
	add constraint "STR_GRP_MTHD_OVRD_FK2" foreign key (
		"STORE_GRADES_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go 
