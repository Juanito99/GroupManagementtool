<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="true" CodeFile="Info.aspx.cs" Inherits="Admin_Info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">

     <asp:UpdatePanel ID="UpdatePanelAll" runat="server" UpdateMode="Conditional">
        <ContentTemplate> 
            <h1>Overview</h1>
            <p></p>
            <table class="tableabout">   
                <tr>
                    <th>Group name:</th>       <th>ManagedBy</th>
                </tr>
                    <asp:Label ID="lblcontentTbl" runat="server" Text="Label"></asp:Label>    
            </table>
            <p></p>
        </ContentTemplate>
    </asp:UpdatePanel>    

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelAll">
        <ProgressTemplate>
            <div class="PleaseWait">
                Loading information ...              
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>          
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpClientScript" Runat="Server">
</asp:Content>

