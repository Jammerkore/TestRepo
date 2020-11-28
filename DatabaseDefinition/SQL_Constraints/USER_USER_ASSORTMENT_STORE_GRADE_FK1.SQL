/* Begin TT#2 - stodd - assortment view selection */
alter table "USER_ASSORTMENT_STORE_GRADE"
	add constraint "USER_USER_ASSORTMENT_STORE_GRADE_FK1" foreign key (
		"USER_RID")
	 references "USER_ASSORTMENT" (
		"USER_RID")  

go
