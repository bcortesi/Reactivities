terraform {
  backend "s3" {
    region       = "us-east-1"
    key          = "mlex.api.state"
    encrypt      = true
    dynamodb_table = "terraform-state-lock"
  }
}