alter table "HEADER_BULK_COLOR_SIZE_NEED"
	add constraint "IB_PROD_HL_HDRBC_SZNEED_FK1" foreign key (
		"IB_MERCH_PH_RID",
		"IB_MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  
go		
