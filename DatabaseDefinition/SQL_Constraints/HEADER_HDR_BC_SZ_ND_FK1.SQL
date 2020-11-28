alter table "HEADER_BULK_COLOR_SIZE_NEED"
    add constraint "HEADER_HDR_BC_SZ_ND_FK1" foreign key (
        "HDR_RID")
	 references "HEADER" (
	    "HDR_RID") 

go
