/*RELEASE 5.4.0.35 12-23-2014 - TT#4250 -stodd - GA - The Tool Bar items need to be locked down.  If you click on the name they move into the GA screen. */
delete from INFRAGISTICS_LAYOUTS where LAYOUT_ID in (38, 42)
GO