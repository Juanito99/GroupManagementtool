<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanelGroups" runat="server" UpdateMode="Conditional">
        <ContentTemplate> 
            <h1>      
                <asp:Label ID="lblGreetingTxt" runat="server" Text="Dear"></asp:Label>   
                <asp:Label ID="lblExistUsrFullName" runat="server" Text="FullName"></asp:Label> 
                <asp:Label ID="lblIntroTxt" runat="server" Text="Update in 3 steps:"></asp:Label>                 
            </h1>
            <p><br /></p>
            <h2>
                <asp:Label ID="lblSelectGroupHeaderTxt" runat="server" Text="Select"></asp:Label>            
            </h2>
            <p></p>
            <p>
                <asp:Label ID="lblSelectedGroupTxt" CssClass="label" runat="server" Text=""></asp:Label>            
                <asp:DropDownList ID="drpdAuthUsrRWGroups" runat="server" CssClass="selects" OnTextChanged="drpdAuthUsrRWGroups_TextChanged" AutoPostBack="True"></asp:DropDownList>
            </p>   
            <p>
                <asp:Label ID="lblGroupNotesTxt" CssClass="label" runat="server" Text="Notes"></asp:Label> 
                <asp:Label ID="lblADGroupsNotes" CssClass="labelNotes" runat="server" Text="&nbsp;"></asp:Label>
            </p>
            <p><br /></p>
            <h2>
                <asp:Label ID="lblMemberHeaderTxt" runat="server" Text="Handling"></asp:Label>            
            </h2>
            <p></p>
            <p>
                <asp:Label ID="lblGroupMembersTxt" CssClass="label" runat="server" Text="Members:"></asp:Label>
                <asp:ListBox ID="lstGroupMembers" runat="server" Rows="8" SelectionMode="Multiple" CssClass="selects" ClientIDMode="Static"></asp:ListBox>    
                <span class="labellft">
                    <asp:LinkButton ID="lbtnRemoveGroupMembers" runat="server" OnClick="lbtnRemoveGroupMembers_Click">Remove Members</asp:LinkButton>
                </span>
            </p>
            <p></p>
            <p> 
                <asp:Label ID="lblMemberSearchTxt" CssClass="label" runat="server" Text="Search:"></asp:Label>              
                <asp:TextBox ID="txtMemberSearch" runat="server" ClientIDMode="Static"></asp:TextBox>                
                <span class="labellft">
                    <asp:LinkButton ID="lbtnAddGroupMembers" runat="server" OnClick="lbtnAddGroupMembers_Click">Add Member:</asp:LinkButton>                
                </span>
                <asp:HiddenField ID="htxtMemberSearch" runat="server" ClientIDMode="Static" />
            </p>
            <p><br /></p>                       
            <h2>
                <asp:Label ID="lblFinishHandlingTxt" runat="server" Text="Finish"></asp:Label>              
            </h2>  
            <p></p>          
            <p>                
                <span class="labellft">
                    <asp:LinkButton ID="lbtnUpdateGroupMembers" runat="server" OnClick="lbtnUpdateGroupMembers_Click">Update Membership</asp:LinkButton>    
                </span>
            </p>                           
                <asp:Label ID="lblAppFeedback" runat="server" Text="" ClientIDMode="Static"></asp:Label>       
            </ContentTemplate>
    </asp:UpdatePanel>    

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelGroups">
        <ProgressTemplate>
            <div class="PleaseWait">              
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>          

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpClientScript" Runat="Server">               
 <script>
     $(document).ready(function () {
        $('[title]').tooltip();

        $("#<%=txtMemberSearch.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/WSautocomplete.asmx/GetUser") %>',                    
                    data: '{filterIn:"' + $("#<%=txtMemberSearch.ClientID%>")[0].value + '" }',
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item,
                                val: item.split(';')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                var cmpkey = i.item.val + ";" + i.item.label;
                $("#<%=htxtMemberSearch.ClientID %>").val(cmpkey);
            },
            minLength: 3
        });

        $("#<%=lstGroupMembers.ClientID %> option:contains(+)").addClass("newgroups");

    });
        

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {        
        $('[title]').tooltip();

        $("#<%=txtMemberSearch.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/WSautocomplete.asmx/GetUser") %>',                    
                    data: '{filterIn:"' + $("#<%=txtMemberSearch.ClientID%>")[0].value + '" }',
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item,
                                val: item.split(';')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                var cmpkey = i.item.val + ";" + i.item.label;
                $("#<%=htxtMemberSearch.ClientID %>").val(cmpkey);
            },
            minLength: 3
        });

        $("#<%=lstGroupMembers.ClientID %> option:contains(+)").addClass("newgroups");
    });
</script>
</asp:Content>

