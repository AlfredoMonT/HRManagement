using Google.Protobuf.WellKnownTypes;

syntax = "proto3";

option csharp_namespace = "HRManagement.WebApi.Protos";

package hr;

// Definición del servicio
service EmployeeGrpc
{
    // Una función para pedir información rápida de un empleado
    rpc GetEmployeeInfo (EmployeeRequest) returns (EmployeeReply);
}

// Mensaje de entrada (lo que envías)
message EmployeeRequest
{
    int32 id = 1;
}

// Mensaje de salida (lo que recibes)
message EmployeeReply
{
    int32 id = 1;
  string fullName = 2;
  string email = 3;
  bool isActive = 4;
}