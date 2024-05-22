

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

 Public Function UnHash(Byval hashstring as UInt32) as String
    Dim id as UInt32 = hashstring Mod &H12345678
    Dim index as UInt32 = 0
    
    For Each c As Char In fileName
        If Char.IsUpper(c) Then
            id = (id + (++index) * (AscW(c) + AscW("a") - AscW("A"))) Mod &H8000000B * &HFFFFFFEF
        Else
            id = (id + (++index) * AscW(c)) Mod &H8000000B * &HFFFFFFEF
        End If
    Next


 End Function

End Class
