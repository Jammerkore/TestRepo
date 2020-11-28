alter table "TASK_FORECAST_BALANCE_DETAIL"
	add constraint "MTHD_TSK_FOR_BAL_DTL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
