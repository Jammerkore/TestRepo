alter table "PROC_HDR"
	add constraint "STATUS_CODE_FK" foreign key (
		"COMPLETION_STATUS_CODE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
