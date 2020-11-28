/* END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35) */
/* Add foreign key constraints to table "METHOD_OVERRIDE_COLOR_MINMAX".                       */
alter table "METHOD_OVERRIDE_COLOR_MINMAX"
	add constraint "MTHD_OVRD_MTHD_OVRD_CLR_MM_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_OVERRIDE" (
		"METHOD_RID")  

go
