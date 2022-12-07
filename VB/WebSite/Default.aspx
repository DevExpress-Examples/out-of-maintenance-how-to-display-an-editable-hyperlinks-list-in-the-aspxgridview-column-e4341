<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		function Employee_Click(id, visibleIndex) {
			alert("Employee with id " + id + " in row " + visibleIndex + " was clicked.");
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<dx:ASPxGridView ID="gvCategories" runat="server" AutoGenerateColumns="False" KeyFieldName="CategoryID"
			EnableRowsCache="false">
			<Columns>
				<dx:GridViewDataTextColumn FieldName="CategoryID" ReadOnly="True" VisibleIndex="0">
					<EditFormSettings Visible="False" />
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="CategoryName" VisibleIndex="1">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataBinaryImageColumn FieldName="Picture" VisibleIndex="2">
				</dx:GridViewDataBinaryImageColumn>
				<dx:GridViewDataTextColumn Caption="Employee" FieldName="Description" VisibleIndex="3">
					<DataItemTemplate>
						<dx:ASPxCallbackPanel ID="cbpnEmployee" runat="server" Width="100%" EnableViewState="false" OnCallback="cbpnEmployee_Callback"
							OnInit="cbpnEmployee_Init">
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<asp:Repeater ID="repEmployee" runat="server" OnInit="repEmployee_Init">
										<HeaderTemplate>
											<table width="100%">
										</HeaderTemplate>
										<ItemTemplate>
											<tr>
												<td>
													<dx:ASPxHyperLink ID="hlEmployee" runat="server" OnInit="hlEmployee_Init">
													</dx:ASPxHyperLink>
												</td>
												<td>
													<dx:ASPxHyperLink ID="hlRemove" runat="server" OnInit="hlRemove_Init" ImageUrl="~/Images/X.png" ImageHeight="16" ImageWidth="16">
													</dx:ASPxHyperLink>
												</td>
											</tr>
										</ItemTemplate>
										<FooterTemplate>
											<tr>
												<td>
													<br />
													<dx:ASPxComboBox ID="cbNewEmployee" runat="server" ValueType="System.Int32" ValueField="EmployeeID"
														OnInit="cbNewEmployee_Init">
														<Columns>
															<dx:ListBoxColumn FieldName="FirstName" />
															<dx:ListBoxColumn FieldName="LastName" />
														</Columns>
													</dx:ASPxComboBox>
													<asp:AccessDataSource ID="AccessDataSource1" runat="server"></asp:AccessDataSource>
													<dx:ASPxHyperLink ID="hlAddNew" runat="server" Text="Add Employee" OnInit="hlAddNew_Init">
													</dx:ASPxHyperLink>
												</td>
												<td>
													<br />
													<dx:ASPxHyperLink ID="hlAdd" runat="server" Text="Add" OnInit="hlAdd_Init">
													</dx:ASPxHyperLink>
													<dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Cancel" OnInit="hlCancel_Init">
													</dx:ASPxHyperLink>
												</td>
											</tr>
											</table>
										</FooterTemplate>
									</asp:Repeater>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
			</Columns>
		</dx:ASPxGridView>
	</div>
	</form>
</body>
</html>