

Public Class Tools


 Public  Function Hash(ByVal fileName As String) As UInteger
    Dim id As UInteger = 0
    Dim index As Integer = 0
    For Each c As Char In fileName
        If Char.IsUpper(c) Then
            id = (id + (++index) * (AscW(c) + AscW("a") - AscW("A"))) Mod &H8000000B * &HFFFFFFEF
        Else
            id = (id + (++index) * AscW(c)) Mod &H8000000B * &HFFFFFFEF
        End If
    Next
    Return (id Xor &H12345678)
 End Function



End Class
