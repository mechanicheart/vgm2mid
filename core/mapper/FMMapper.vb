Namespace Core
    Namespace FM
        Public MustInherit Class FMMapper : Inherits MemoryMapper
            Private Enum FMMapperIDType
                AR
                D1R
                D2R
                SR
                RR
                D1L
                D2L
                TL
                SL
            End Enum
            Public FMMapperID As Long() = {
                FMMapperIDType.AR, FMMapperIDType.D1R, FMMapperIDType.D2R, FMMapperIDType.SR, FMMapperIDType.RR,
                FMMapperIDType.D1L, FMMapperIDType.D2L, FMMapperIDType.TL, FMMapperIDType.SL
            }

            Public FMMapperString As String() = {
                "AR", "D1R", "D2R", "SR", "RR",
                "D1L", "D2L", "TL", "SL"
            }

        End Class

        Namespace OPM
            Public Class OPMMapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class
        End Namespace

        Namespace OPN
            Public Class OPNMapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class
            Public Class OPNAMapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class

            Public Class OPNBMapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class

            Public Class OPN2Mapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class

            Public Class OPNCMapper : Inherits FMMapper
                Public Overrides Sub Init()

                End Sub
            End Class
        End Namespace
    End Namespace
End Namespace
