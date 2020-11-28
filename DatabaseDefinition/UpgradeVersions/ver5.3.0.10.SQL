/*RELEASE 5.3.0.10 06-17-2013 - TT#767-MD -stodd - remove old controls from allocation workspace*/
delete from INFRAGISTICS_LAYOUTS where LAYOUT_ID = 36 --Remove allocation workspace toolbars layout
delete from INFRAGISTICS_LAYOUTS where LAYOUT_ID = 37 --Remove Assortment workspace toolbars layout

GO