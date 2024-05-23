

Public Class Tools


 Public  Function Hash(ByVal fileName As String) As UInt64
    Dim id As UInt64 = 0
    Dim index As Integer = 0
    For Each c As Char In fileName
        If (AscW(c) >= AscW("A")) AndAlso (AscW(c) <= AscW("Z")) Then
            id = (id + (index+1) * (AscW(c) + AscW("a") - AscW("A"))) Mod &H8000000B * &HFFFFFFEF
        Else
            id = (id + (index+1) * AscW(c)) Mod &H8000000B * &HFFFFFFEF
        End If
    Next
    Return (id Xor &H12345678)
 End Function

 Public Function UnHash(Byval hashstring as UInt64) as String
    Dim id as UInt64 = 0
    Dim index as UInt32 = 0
    Dim temp as UInt64 = hashstring Mod &H12345678
    For Each c As Char In temp
        If Char.IsUpper(c) Then
            id = (id - (++index) / (AscW(c) - AscW("a") + AscW("A"))) Xor &H8000000B / &HFFFFFFEF
        Else
            id = (id - (++index) / AscW(c)) Xor &H8000000B / &HFFFFFFEF
        End If
    Next
    Return Cstr(id)
 End Function

End Class
