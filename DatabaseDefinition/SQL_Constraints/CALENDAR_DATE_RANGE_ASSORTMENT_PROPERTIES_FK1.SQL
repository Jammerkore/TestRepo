/* Added for TT#2 - stodd - assortment */
alter table "ASSORTMENT_PROPERTIES"
	add constraint "CALENDAR_DATE_RANGE_ASSORTMENT_PROPERTIES_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID") on update no action on delete no action  
go
