<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128539275/12.1.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4341)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [DataHelper.cs](./CS/WebSite/App_Code/DataHelper.cs) (VB: [DataHelper.vb](./VB/WebSite/App_Code/DataHelper.vb))
* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# How to display an editable hyperlinks list in the ASPxGridView column
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/e4341/)**
<!-- run online end -->


<p>This example demonstrates how you can display cell data as a list of clickable hyperlinks, with capability to remove and add new items.</p><br />
<p>To accomplish this task, use the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxGridViewGridViewDataColumn_DataItemTemplatetopic"><u>GridViewDataColumn.DataItemTemplate</u></a> template. To display a hyperlinks list, use a standard ASP.NET <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.repeater.aspx"><u>Repeater</u></a> control placed inside the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument8277"><u>ASPxCallbackPanel</u></a>. The ASPxCallbackPanel is used to update an ASP.NET Repeater on callbacks. Handle its <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxCallbackPanelASPxCallbackPanel_Callbacktopic"><u>ASPxCallbackPanel.Callback</u></a> event to perform data updates (adding and deleting items) and mode changes (switching to the edit mode and back).</p><p><strong>See also:<br />
</strong><a href="https://www.devexpress.com/Support/Center/p/K18282">The general technique of using the Init/Load event handler</a><br />
<a href="https://www.devexpress.com/Support/Center/p/E2333">How to perform ASPxGridView instant updating using different editors in the DataItem template</a></p><p><strong>P.S.</strong> The last Employee cannot be removed from the row, because each row should have at least one employee.</p>

<br/>


