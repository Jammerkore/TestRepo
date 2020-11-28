alter table "PROC_RPT"
	add constraint "APPLICATION_TEXT_PROC_RPT_FK1" foreign key (
		"MESSAGE_CODE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
