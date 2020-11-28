alter table "PROC_HDR"
	add constraint "APPLICATION_TEXT_PROC_HDR_FK3" foreign key (
		"HIGHEST_LEVEL")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
