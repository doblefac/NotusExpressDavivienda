Imports LMDataAccessLayer
Imports EncryptionClassLibrary
Imports NotusExpressBusinessLayer.General

Namespace ControlAcceso

    Public Class ValidadorAcceso

#Region "Atributos (Campos)"

        Private _usuario As String
        Private _password As String
        Private _latitud As String
        Private _longitud As String
        Private _idUsuario As Integer
        Private _esPresencial As Boolean
        Private _idPerfil As Integer
        Private _idCiudad As Integer

#End Region

#Region "Propiedades"

        Public Property Usuario() As String
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property Latitud() As String
            Get
                Return _latitud
            End Get
            Set(value As String)
                _latitud = value
            End Set
        End Property

        Public Property Longitud() As String
            Get
                Return _longitud
            End Get
            Set(value As String)
                _longitud = value
            End Set
        End Property

        Public Property EsPresencial() As Boolean
            Get
                Return _esPresencial
            End Get
            Set(value As Boolean)
                _esPresencial = value
            End Set
        End Property

        Public ReadOnly Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
        End Property

        Public ReadOnly Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
        End Property

        Public ReadOnly Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
        End Property

#End Region

#Region "Métodos Privados"


#End Region

#Region "Métodos Públicos"

        Public Function EsUsuarioValido(ByVal identificacion As String) As Boolean
            Dim dbManager As New LMDataAccess
            Dim resultado As Byte = 1
            Try
                With dbManager
                    .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 100).Value = identificacion
                    .SqlParametros.Add("@esPresencial", SqlDbType.Bit).Value = _esPresencial
                    .SqlParametros.Add("@latitud", SqlDbType.VarChar, 20).Value = _latitud
                    .SqlParametros.Add("@longitud", SqlDbType.VarChar, 20).Value = _longitud
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("ValidarCredencialesDeAcceso", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                        Integer.TryParse(.SqlParametros("@idUsuario").Value.ToString, _idUsuario)
                        If _idUsuario > 0 Then
                            Dim usuario As New UsuarioSistema(_idUsuario)
                            _idPerfil = usuario.IdPerfil
                            _idCiudad = usuario.IdCiudad
                        Else
                            resultado = 1
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return Not CBool(resultado)
        End Function

        Public Function EsUsuarioValidoPresencial(ByVal pwdEncriptado As String) As ResultadoProceso
            Dim _Resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            'Dim resultado As Byte = 1
            Try
                With dbManager
                    .SqlParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = _usuario
                    .SqlParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = pwdEncriptado
                    .SqlParametros.Add("@esPresencial", SqlDbType.Bit).Value = _esPresencial
                    .SqlParametros.Add("@latitud", SqlDbType.VarChar, 20).Value = _latitud
                    .SqlParametros.Add("@longitud", SqlDbType.VarChar, 20).Value = _longitud
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@idPerfil", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("ValidarCredencialesDeAcceso", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        _Resultado.Valor = CInt(.SqlParametros("@resultado").Value.ToString)
                        Integer.TryParse(.SqlParametros("@idUsuario").Value.ToString, _idUsuario)
                        Integer.TryParse(.SqlParametros("@idPerfil").Value.ToString, _idPerfil)
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            'Return Not CBool(resultado)
            Return _Resultado
        End Function

#End Region


    End Class

End Namespace