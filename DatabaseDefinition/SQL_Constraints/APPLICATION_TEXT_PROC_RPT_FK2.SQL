alter table "PROC_RPT"
	add constraint "APPLICATION_TEXT_PROC_RPT_FK2" foreign key (
		"MESSAGE_LEVEL")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
