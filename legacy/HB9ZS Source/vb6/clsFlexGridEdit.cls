VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsFlexGridEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'===========================================0
' Klassen-Modul für die Texteingabe im Flexgrid
'
'wird vom Flexgid aufgerufen
'17.03.2003 / hb9zs
'===============================================

Option Explicit

Public Enum FlexEditKeyBehaviour
   fgEditNone = 0
   fgEditMoveRight = 1
   fgEditMoveDown = 2
   fgEditMoveup = 3
End Enum

Private Type cntlInfo
      blnIsArray As Boolean
      lngIndex As Long
      strName As String
      blntabStop As Boolean
End Type

Private WithEvents Fg As MSFlexGrid
Attribute Fg.VB_VarHelpID = -1
Private WithEvents Tb As TextBox
Attribute Tb.VB_VarHelpID = -1

Private strTbName As String
Private prntCntls() As cntlInfo
Private m_TbBorderStyle As AppearanceSettings
Private m_EnterKeyBehaviour As FlexEditKeyBehaviour
Private m_TabKeyBehaviour As FlexEditKeyBehaviour
Private m_blnMoving As Boolean

Public Property Set FlexGridControl(fgControl As MSFlexGrid)
   
   RemoveOldTextBox
   Set Fg = fgControl
   On Error Resume Next
      strTbName = "tbFgEdit"
      Do
         Err = 0
         strTbName = strTbName & "1"
         Fg.Parent.Controls.Add "VB.TextBox", strTbName, Fg.Container
      Loop While Err <> 0
   Set Tb = Fg.Parent.Controls(strTbName)
   With Tb
      .Visible = False
      .BorderStyle = m_TbBorderStyle
      Set .Font = Fg.Font
      .BackColor = &H80C0FF
      .TabStop = False
      .ZOrder
   End With
   
End Property


Public Property Let EditBoxBorderStyle(varBorderStyle As AppearanceSettings)
   
   m_TbBorderStyle = varBorderStyle
   If Not Tb Is Nothing Then Tb.BorderStyle = m_TbBorderStyle

End Property


Public Property Let EnterKeyBehaviour(varKeyBehaviour As FlexEditKeyBehaviour)
   
   m_EnterKeyBehaviour = varKeyBehaviour

End Property


Public Property Let TabKeyBehaviour(varKeyBehaviour As FlexEditKeyBehaviour)
   
   ' need to restore tabstops to parent controls if varKeyBehaviour = 0
   If varKeyBehaviour = fgEditNone Then
      If m_TabKeyBehaviour <> fgEditNone Then RestoreTabStops
   End If
   m_TabKeyBehaviour = varKeyBehaviour

End Property


Private Sub Class_Initialize()
   
   ReDim prntCntls(0)

End Sub


Private Sub Class_Terminate()
   
   RemoveOldTextBox

End Sub



'++++++++++++++++++++++++++++++
'flex grid events

Private Sub fg_DblClick()
   
   fgInitEdit 32

End Sub


Private Sub Fg_KeyPress(KeyAscii As Integer)
   
   fgInitEdit KeyAscii

End Sub


Private Sub fg_GotFocus()
   
   UpdateFg
   
'Sez für Kabelflexgrid2
'    frmMain.Kabel_berechnen

End Sub


Private Sub fg_LeaveCell()
   
   If Not m_blnMoving Then UpdateFg

End Sub


Private Sub fg_Scroll()
  
  Dim dx As Long, dy As Long
  ' move tb with cell or hide if cell out of view
  If Tb.Visible Then
    With Fg
      If .RowIsVisible(.Row) And .ColIsVisible(.Col) Then
'        dx = .Left + .Container.ScaleX(.CellLeft, vbTwips, .Container.ScaleMode)
        dx = .Left + .CellLeft
'        dy = .Top + .Container.ScaleY(.CellTop, vbTwips, .Container.ScaleMode)
        dy = .Top + .CellTop
        Tb.Move dx, dy
      Else
        Tb.Move -Tb.Width, -Tb.Height
      End If
    End With
  End If

End Sub

'+++++++++++++++++++++++
' Text box events

Private Sub Tb_KeyPress(KeyAscii As Integer)
   
    'Spez für Kabeleingabe

If frmMain.FlexKabel2.Col = 1 Then
    
Else

    
    'Zeicheneingabe auf Zahlen begrenzen und Punkt statt Komma
    Select Case KeyAscii
        Case Asc("0") To Asc("9")
        Case Asc(","), Asc(".")
        If InStr(Tb.Text, ",") <> 0 Then
            KeyAscii = 0
        Else
            KeyAscii = Asc(".")
        End If
        Case Asc(vbBack)
        Case Else
        KeyAscii = 0
    End Select
    'Spezial Ende

   '  get rid of beeps.
   Select Case KeyAscii
      Case 9, 13, 27
         KeyAscii = 0
   End Select

End If

End Sub


Private Sub tb_KeyDown(KeyCode As Integer, Shift As Integer)
   
   Dim i As Long
   'read cellTop & cellLeft properties to force into view
   If (Tb.Left < 0) Or (Tb.Top < 0) Then
       i = Fg.CellTop
       i = Fg.CellLeft
   End If
   
   Select Case KeyCode

   Case 9  ' tab key
      MoveToCell m_TabKeyBehaviour
   
   Case 13   ' ENTER key
      MoveToCell m_EnterKeyBehaviour

   Case 27   ' ESC
      Tb.Visible = False
      Fg.SetFocus
 
   Case 38      ' Up.
      MoveToCell fgEditMoveup

   Case 40      ' Down.
      MoveToCell fgEditMoveDown
   End Select

End Sub


Private Sub Tb_LostFocus()
   
   RestoreTabStops

End Sub


' +++++++++++++++++++++++
' Utility functions

Private Sub fgInitEdit(KeyAscii As Integer)
   
    'Spezial
    blk = 1 'Blinker für Berechnung starten
    
'Spez für Kabeleingabe
If frmMain.FlexKabel2.Col = 1 Then
    
Else

    'Zeicheneingabe auf Zahlen begrenzen und Punkt statt Komma
    Select Case KeyAscii
        Case Asc("0") To Asc("9")
        Case Asc(","), Asc(".")
        If InStr(Fg.Text, ",") <> 0 Then
            KeyAscii = 0
        Else
            KeyAscii = Asc(".")
        End If
        Case Asc(vbBack)
        Case Else
        KeyAscii = 0
    End Select
    'Spezial Ende
End If
   Dim sngL As Single, sngT As Single, sngW As Single, sngH As Single
   Select Case KeyAscii
   ' A space means edit the current text.
   Case 0 To 32
      Tb = Fg.Text
      Tb.SelStart = 0
      Tb.SelLength = Len(Tb.Text)
   ' Anything else means replace the current text.
   Case Else
      Tb = Chr(KeyAscii)
      Tb.SelStart = Len(Tb.Text)
   End Select
   
   With Fg.Container
      sngL = Fg.CellLeft  '.ScaleX(Fg.CellLeft, vbTwips, .ScaleMode)
      sngT = Fg.CellTop   '.ScaleY(Fg.CellTop, vbTwips, .ScaleMode)
      sngW = Fg.CellWidth  ' .ScaleX(Fg.CellWidth, vbTwips, .ScaleMode)
      sngH = Fg.CellHeight '.ScaleY(Fg.CellHeight, vbTwips, .ScaleMode)
   End With
   ' Show textbox at the right place.
   
   Tb.Move Fg.Left + sngL, Fg.Top + sngT, sngW, sngH
   Tb.Visible = True
   Tb.SetFocus
   If m_TabKeyBehaviour <> fgEditNone Then
      RestoreTabStops
      RemoveTabStops
   End If

End Sub


Private Sub MoveToCell(varMoveBehaviour As FlexEditKeyBehaviour)
   
   m_blnMoving = True
   Select Case varMoveBehaviour
   
   Case fgEditNone
      Fg.SetFocus
   
   Case fgEditMoveDown
      Fg.Text = Tb.Text
      If Fg.Row + 1 < Fg.Rows Then
         Fg.Row = Fg.Row + 1
      ElseIf Fg.Col + 1 < Fg.Cols Then
         Fg.Col = Fg.Col + 1
         Fg.Row = Fg.FixedRows
      Else
         Fg.Col = Fg.FixedCols
         Fg.Row = Fg.FixedRows
      End If
      fgInitEdit 0
   
   Case fgEditMoveRight
      Fg.Text = Tb.Text
      If Fg.Col + 1 < Fg.Cols Then
         Fg.Col = Fg.Col + 1
      ElseIf Fg.Row + 1 < Fg.Rows Then
         Fg.Row = Fg.Row + 1
         Fg.Col = Fg.FixedCols
      Else
         Fg.Col = Fg.FixedCols
         Fg.Row = Fg.FixedRows
      End If
      fgInitEdit 0

   Case fgEditMoveup
      Fg.Text = Tb.Text
      If Fg.Row > Fg.FixedRows Then
         Fg.Row = Fg.Row - 1
      ElseIf Fg.Col > Fg.FixedCols Then
         Fg.Col = Fg.Col - 1
         Fg.Row = Fg.Rows - 1
      Else
         Fg.Col = Fg.Cols - 1
         Fg.Row = Fg.Rows - 1
      End If
      fgInitEdit 0

   End Select
   m_blnMoving = False

End Sub


Private Sub RemoveTabStops()
   
   Dim cntl As Control, i As Long
   On Error Resume Next
   With Fg.Parent
      ReDim prntCntls(.Controls.Count)
      For Each cntl In .Controls
         i = i + 1
         If TypeName(.Controls(cntl.Name)) = "Object" Then
            prntCntls(i).blnIsArray = True
            prntCntls(i).lngIndex = cntl.Index
         End If
         prntCntls(i).strName = cntl.Name
         prntCntls(i).blntabStop = cntl.TabStop
         cntl.TabStop = False
      Next
   End With

End Sub


Private Sub RestoreTabStops()
   
   Dim i As Long
   If Fg Is Nothing Then Exit Sub
   On Error Resume Next
   With Fg.Parent
      For i = 1 To UBound(prntCntls)
         If prntCntls(i).blnIsArray Then
            .Controls(prntCntls(i).strName)(prntCntls(i).lngIndex).TabStop = prntCntls(i).blntabStop
         Else
            .Controls(prntCntls(i).strName).TabStop = prntCntls(i).blntabStop
         End If
      Next
   End With
   ReDim prntCntls(0)

End Sub


Private Sub RemoveOldTextBox()
   
   On Error Resume Next
   If Not Fg Is Nothing Then
     Set Tb = Nothing
     Fg.Parent.Controls.Remove strTbName
     Set Fg = Nothing
   End If

End Sub


Private Sub UpdateFg()
   
   If Tb.Visible = False Then Exit Sub
   Fg.Text = Tb.Text
   Tb.Visible = False

End Sub
'_____________________________________________________________________




