/* begin TT#41 - JEllis - MD - UC2 - Size Inventory Min Max pt 1 */
alter table "HEADER_BULK_COLOR_SIZE_NEED"
	add constraint "IB_HN_HDRBC_SZNEED_FK1" foreign key (
		"IB_MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
