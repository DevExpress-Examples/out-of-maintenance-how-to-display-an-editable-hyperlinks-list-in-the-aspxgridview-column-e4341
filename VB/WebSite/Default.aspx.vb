Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxCallbackPanel
Imports System.Data

Partial Public Class _Default
	Inherits System.Web.UI.Page
	#Region "Example Data"
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		If Session("GridData") Is Nothing Then
			Dim gridData As DataTable = DataHelper.ProcessSelectCommand("SELECT [CategoryID], [CategoryName], [Picture], [Description] FROM [Categories]")
			gridData.PrimaryKey = New DataColumn() { gridData.Columns(gvCategories.KeyFieldName) }
			Session("GridData") = gridData
		End If

		gvCategories.DataSource = Session("GridData")
		gvCategories.DataBind()
	End Sub

	Private Sub UpdateEmployeeCell(ByVal rowKey As Object, ByVal newValue As String)
		Dim gridData As DataTable = TryCast(Session("GridData"), DataTable)
		Dim row As DataRow = gridData.Rows.Find(rowKey)
		row("Description") = newValue
	End Sub

	#End Region

	#Region "Common"
	Private Function GetEmployeeData(ByVal employeeList As String) As DataTable
		Dim employees() As String = employeeList.Split(","c)
		Dim selectCommand As String = String.Format("SELECT [EmployeeID], [FirstName], [LastName] FROM [Employees] WHERE ([EmployeeID] IN ({0}))", String.Join(",", employees))
		Return DataHelper.ProcessSelectCommand(selectCommand)
	End Function

	Private Function GetEditMode(ByVal callbackPanel As ASPxCallbackPanel) As Boolean
		Return callbackPanel.JSProperties.ContainsKey("cpIsEditing")
	End Function

	Private Sub SetEditMode(ByVal callbackPanel As ASPxCallbackPanel, ByVal inEditMode As Boolean)
		If inEditMode <> GetEditMode(callbackPanel) Then
			If inEditMode Then
				callbackPanel.JSProperties.Add("cpIsEditing", True)
			Else
				callbackPanel.JSProperties.Remove("cpIsEditing")
			End If
		End If
	End Sub
	#End Region

	#Region "Controls Initialization"

	Protected Sub cbpnEmployee_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
		Dim container As GridViewDataItemTemplateContainer = TryCast(callbackPanel.NamingContainer, GridViewDataItemTemplateContainer)

		callbackPanel.ClientInstanceName = String.Format("callbackPanel_{0}", container.KeyValue)
	End Sub

	Protected Sub repEmployee_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim rep As Repeater = TryCast(sender, Repeater)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(rep.NamingContainer, ASPxCallbackPanel)
		Dim gridContainer As GridViewDataItemTemplateContainer = TryCast(callbackPanel.NamingContainer, GridViewDataItemTemplateContainer)

		Dim cellData As String = DataBinder.Eval(gridContainer.DataItem, gridContainer.Column.FieldName).ToString()

		rep.DataSource = GetEmployeeData(cellData)
	End Sub

	Protected Sub hlEmployee_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
		Dim item As RepeaterItem = TryCast(link.NamingContainer, RepeaterItem)
		Dim gridContainer As GridViewDataItemTemplateContainer = TryCast(item.NamingContainer.NamingContainer.NamingContainer, GridViewDataItemTemplateContainer)

		Dim employeeID As Object = DataBinder.Eval(item.DataItem, "EmployeeID")

		link.Text = String.Format("{0} {1}", DataBinder.Eval(item.DataItem, "FirstName"), DataBinder.Eval(item.DataItem, "LastName"))
		link.ClientSideEvents.Click = String.Format("function(s, e) {{ Employee_Click({0}, {1}); }}", employeeID, gridContainer.VisibleIndex)
	End Sub

	Protected Sub hlRemove_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
		Dim item As RepeaterItem = TryCast(link.NamingContainer, RepeaterItem)
		Dim rep As Repeater = TryCast(item.NamingContainer, Repeater)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(rep.NamingContainer, ASPxCallbackPanel)

		Dim employeeID As Object = DataBinder.Eval(item.DataItem, "EmployeeID")

		link.ClientSideEvents.Click = String.Format("function(s, e) {{ if (confirm('Are you sure?')) {0}.PerformCallback('DELETE|{1}'); }}", callbackPanel.ClientInstanceName, employeeID)
	End Sub

	Protected Sub cbNewEmployee_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim comboBox As ASPxComboBox = TryCast(sender, ASPxComboBox)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(comboBox.NamingContainer.NamingContainer.NamingContainer, ASPxCallbackPanel)
		Dim gridContainer As GridViewDataItemTemplateContainer = TryCast(callbackPanel.NamingContainer, GridViewDataItemTemplateContainer)

		comboBox.Visible = GetEditMode(callbackPanel)
		comboBox.ClientInstanceName = String.Format("cbNewEmployee_{0}", gridContainer.KeyValue)

		Dim employeeData As String = DataBinder.Eval(gridContainer.DataItem, gridContainer.Column.FieldName).ToString()
		Dim employee() As String = employeeData.Split(","c)
		Dim selectCommand As String = String.Format("SELECT [EmployeeID], [FirstName], [LastName] FROM [Employees] WHERE (NOT [EmployeeID] IN ({0}))", String.Join(",", employee))
		comboBox.DataSource = DataHelper.ProcessSelectCommand(selectCommand)
		comboBox.DataBind()
	End Sub

	Protected Sub hlAddNew_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(link.NamingContainer.NamingContainer.NamingContainer, ASPxCallbackPanel)

		link.Visible = Not GetEditMode(callbackPanel)
		link.ClientSideEvents.Click = String.Format("function(s, e) {{ {0}.PerformCallback('ADDNEW'); }}", callbackPanel.ClientInstanceName)
	End Sub

	Protected Sub hlAdd_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(link.NamingContainer.NamingContainer.NamingContainer, ASPxCallbackPanel)
		Dim gridContainer As GridViewDataItemTemplateContainer = TryCast(callbackPanel.NamingContainer, GridViewDataItemTemplateContainer)

		link.Visible = GetEditMode(callbackPanel)
		link.ClientSideEvents.Click = String.Format("function(s, e) {{ var value = cbNewEmployee_{0}.GetValue(); if (value != null) {1}.PerformCallback('ADD|' + value); }}", gridContainer.KeyValue, callbackPanel.ClientInstanceName)
	End Sub

	Protected Sub hlCancel_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(link.NamingContainer.NamingContainer.NamingContainer, ASPxCallbackPanel)

		link.Visible = GetEditMode(callbackPanel)
		link.ClientSideEvents.Click = String.Format("function(s, e) {{ {0}.PerformCallback('CANCEL'); }}", callbackPanel.ClientInstanceName)
	End Sub
	#End Region

	Protected Sub cbpnEmployee_Callback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxClasses.CallbackEventArgsBase)
		Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
		Dim container As GridViewDataItemTemplateContainer = TryCast(callbackPanel.NamingContainer, GridViewDataItemTemplateContainer)
		Dim repEmployee As Repeater = TryCast(callbackPanel.FindControl("repEmployee"), Repeater)

		Dim parameters() As String = e.Parameter.Split("|"c)
		Dim operation As String = parameters(0)

		Dim cellData As String = DataBinder.Eval(container.DataItem, container.Column.FieldName).ToString()
        Dim employees As List(Of String) = New List(Of String)(cellData.Split(","c))
		Dim isDataUpdated As Boolean = False

		Select Case operation
			Case "DELETE"
				If employees.Count > 1 Then
					Dim id As String = parameters(1)
					employees.Remove(id)
				End If

				isDataUpdated = True
			Case "ADD"
				Dim newEmployee As String = parameters(1)
				employees.Add(newEmployee)

				isDataUpdated = True
				SetEditMode(callbackPanel, False)
			Case "ADDNEW"
				SetEditMode(callbackPanel, True)
			Case "CANCEL"
				SetEditMode(callbackPanel, False)
		End Select


		Dim employeesList As String = String.Join(",", employees.ToArray())

		If isDataUpdated Then
			'Update grid's data
			UpdateEmployeeCell(container.KeyValue, employeesList)
		End If

		repEmployee.DataSource = GetEmployeeData(employeesList)
		repEmployee.DataBind()
	End Sub
End Class