variable "environment" {
  type = string
}

variable "project" {
  type = string
}

variable "region" {
  type = string
}

variable "sqlsvr_name" {
  type = string
}

variable "sql_version" {
  type = string
}

variable "sql_admin_username" {
  type = string
}

variable "sql_admin_password" {
  type = string
}

variable "sqladmins" {
  type = string
}

variable "bjssvpn" {
  type = string
}

variable "subnet_backend_id" {
  type = string
  default = ""
}