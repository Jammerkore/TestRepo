alter table "PROC_HDR"
	add constraint "APPLICATION_TEXT_PROC_HDR_FK2" foreign key (
		"SUMMARY_CODE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
