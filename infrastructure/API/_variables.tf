variable "profile" {
  default     = ""
  description = "Overwrite this value if not running from an ec2 instance with an IAM role"
}
variable "region" {
  default     = "us-east-1"
  description = "AWS region"
}