using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;

public partial class _Default : System.Web.UI.Page {
    #region Example Data
    protected void Page_Init(object sender, EventArgs e) {
        if (Session["GridData"] == null) {
            DataTable gridData = DataHelper.ProcessSelectCommand("SELECT [CategoryID], [CategoryName], [Picture], [Description] FROM [Categories]");
            gridData.PrimaryKey = new DataColumn[] { gridData.Columns[gvCategories.KeyFieldName] };
            Session["GridData"] = gridData;
        }

        gvCategories.DataSource = Session["GridData"];
        gvCategories.DataBind();
    }

    void UpdateEmployeeCell(object rowKey, string newValue) {
        DataTable gridData = Session["GridData"] as DataTable;
        DataRow row = gridData.Rows.Find(rowKey);
        row["Description"] = newValue;
    }

    #endregion

    #region Common
    DataTable GetEmployeeData(string employeeList) {
        string[] employees = employeeList.Split(',');
        string selectCommand = String.Format("SELECT [EmployeeID], [FirstName], [LastName] FROM [Employees] WHERE ([EmployeeID] IN ({0}))", String.Join(",", employees));
        return DataHelper.ProcessSelectCommand(selectCommand);
    }

    bool GetEditMode(ASPxCallbackPanel callbackPanel) {
        return callbackPanel.JSProperties.ContainsKey("cpIsEditing");
    }

    void SetEditMode(ASPxCallbackPanel callbackPanel, bool inEditMode) {
        if (inEditMode != GetEditMode(callbackPanel)) {
            if (inEditMode) {
                callbackPanel.JSProperties.Add("cpIsEditing", true);
            }
            else {
                callbackPanel.JSProperties.Remove("cpIsEditing");
            }
        }
    }    
    #endregion

    #region Controls Initialization

    protected void cbpnEmployee_Init(object sender, EventArgs e) {
        ASPxCallbackPanel callbackPanel = sender as ASPxCallbackPanel;
        GridViewDataItemTemplateContainer container = callbackPanel.NamingContainer as GridViewDataItemTemplateContainer;

        callbackPanel.ClientInstanceName = String.Format("callbackPanel_{0}", container.KeyValue);
    }

    protected void repEmployee_Init(object sender, EventArgs e) {
        Repeater rep = sender as Repeater;
        ASPxCallbackPanel callbackPanel = rep.NamingContainer as ASPxCallbackPanel;
        GridViewDataItemTemplateContainer gridContainer = callbackPanel.NamingContainer as GridViewDataItemTemplateContainer;

        string cellData = DataBinder.Eval(gridContainer.DataItem, gridContainer.Column.FieldName).ToString();

        rep.DataSource = GetEmployeeData(cellData);
    }

    protected void hlEmployee_Init(object sender, EventArgs e) {
        ASPxHyperLink link = sender as ASPxHyperLink;
        RepeaterItem item = link.NamingContainer as RepeaterItem;
        GridViewDataItemTemplateContainer gridContainer = item.NamingContainer.NamingContainer.NamingContainer as GridViewDataItemTemplateContainer;

        object employeeID = DataBinder.Eval(item.DataItem, "EmployeeID");

        link.Text = String.Format("{0} {1}", DataBinder.Eval(item.DataItem, "FirstName"), DataBinder.Eval(item.DataItem, "LastName"));
        link.ClientSideEvents.Click = String.Format("function(s, e) {{ Employee_Click({0}, {1}); }}", employeeID, gridContainer.VisibleIndex);
    }

    protected void hlRemove_Init(object sender, EventArgs e) {
        ASPxHyperLink link = sender as ASPxHyperLink;
        RepeaterItem item = link.NamingContainer as RepeaterItem;
        Repeater rep = item.NamingContainer as Repeater;
        ASPxCallbackPanel callbackPanel = rep.NamingContainer as ASPxCallbackPanel;

        object employeeID = DataBinder.Eval(item.DataItem, "EmployeeID");

        link.ClientSideEvents.Click = String.Format("function(s, e) {{ if (confirm('Are you sure?')) {0}.PerformCallback('DELETE|{1}'); }}", callbackPanel.ClientInstanceName, employeeID);
    }

    protected void cbNewEmployee_Init(object sender, EventArgs e) {
        ASPxComboBox comboBox = sender as ASPxComboBox;
        ASPxCallbackPanel callbackPanel = comboBox.NamingContainer.NamingContainer.NamingContainer as ASPxCallbackPanel;
        GridViewDataItemTemplateContainer gridContainer = callbackPanel.NamingContainer as GridViewDataItemTemplateContainer;

        comboBox.Visible = GetEditMode(callbackPanel);
        comboBox.ClientInstanceName = String.Format("cbNewEmployee_{0}", gridContainer.KeyValue);

        string employeeData = DataBinder.Eval(gridContainer.DataItem, gridContainer.Column.FieldName).ToString();
        string[] employee = employeeData.Split(',');
        string selectCommand = String.Format("SELECT [EmployeeID], [FirstName], [LastName] FROM [Employees] WHERE (NOT [EmployeeID] IN ({0}))", String.Join(",", employee));
        comboBox.DataSource = DataHelper.ProcessSelectCommand(selectCommand);
        comboBox.DataBind();
    }

    protected void hlAddNew_Init(object sender, EventArgs e) {
        ASPxHyperLink link = sender as ASPxHyperLink;
        ASPxCallbackPanel callbackPanel = link.NamingContainer.NamingContainer.NamingContainer as ASPxCallbackPanel;

        link.Visible = !GetEditMode(callbackPanel);
        link.ClientSideEvents.Click = String.Format("function(s, e) {{ {0}.PerformCallback('ADDNEW'); }}", callbackPanel.ClientInstanceName);
    }

    protected void hlAdd_Init(object sender, EventArgs e) {
        ASPxHyperLink link = sender as ASPxHyperLink;
        ASPxCallbackPanel callbackPanel = link.NamingContainer.NamingContainer.NamingContainer as ASPxCallbackPanel;
        GridViewDataItemTemplateContainer gridContainer = callbackPanel.NamingContainer as GridViewDataItemTemplateContainer;

        link.Visible = GetEditMode(callbackPanel);
        link.ClientSideEvents.Click = String.Format("function(s, e) {{ var value = cbNewEmployee_{0}.GetValue(); if (value != null) {1}.PerformCallback('ADD|' + value); }}", gridContainer.KeyValue, callbackPanel.ClientInstanceName);
    }

    protected void hlCancel_Init(object sender, EventArgs e) {
        ASPxHyperLink link = sender as ASPxHyperLink;
        ASPxCallbackPanel callbackPanel = link.NamingContainer.NamingContainer.NamingContainer as ASPxCallbackPanel;

        link.Visible = GetEditMode(callbackPanel);
        link.ClientSideEvents.Click = String.Format("function(s, e) {{ {0}.PerformCallback('CANCEL'); }}", callbackPanel.ClientInstanceName);
    }
    #endregion

    protected void cbpnEmployee_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e) {
        ASPxCallbackPanel callbackPanel = sender as ASPxCallbackPanel;
        GridViewDataItemTemplateContainer container = callbackPanel.NamingContainer as GridViewDataItemTemplateContainer;
        Repeater repEmployee = callbackPanel.FindControl("repEmployee") as Repeater;

        string[] parameters = e.Parameter.Split('|');
        string operation = parameters[0];

        string cellData = DataBinder.Eval(container.DataItem, container.Column.FieldName).ToString();
        List<string> employees = cellData.Split(',').ToList<string>();
        bool isDataUpdated = false;

        switch (operation) {
            case "DELETE":
                if (employees.Count > 1) {
                    string id = parameters[1];
                    employees.Remove(id);
                }

                isDataUpdated = true;
                break;
            case "ADD":
                string newEmployee = parameters[1];
                employees.Add(newEmployee);

                isDataUpdated = true;
                SetEditMode(callbackPanel, false);
                break;
            case "ADDNEW":
                SetEditMode(callbackPanel, true);
                break;
            case "CANCEL":
                SetEditMode(callbackPanel, false);
                break;
        }


        string employeesList = String.Join(",", employees.ToArray());

        if (isDataUpdated) {
            //Update grid's data
            UpdateEmployeeCell(container.KeyValue, employeesList);            
        }
        
        repEmployee.DataSource = GetEmployeeData(employeesList);
        repEmployee.DataBind();
    }
}