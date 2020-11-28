alter table "HEADER_BULK_COLOR_SIZE_NEED"
    add constraint "HDR_BC_HDR_BC_SZ_ND_FK1" foreign key (
        "HDR_BC_RID")
	 references "HEADER_BULK_COLOR" (
	    "HDR_BC_RID") 

go
