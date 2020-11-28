alter table "TASK_ALLOCATE_DETAIL"
	add constraint "METHOD_TASK_ALLOCATE_DTL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
