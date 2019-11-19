resource "aws_vpc" "mlex" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name    = "MLex",
    AssetID = "2652"
  }
}

resource "aws_internet_gateway" "mlex" {
  vpc_id = "${aws_vpc.mlex.id}"
  tags = {
    Name    = "MLex",
    AssetID = "2652"
  }
}

resource "aws_subnet" "mlex-public-a" {
  vpc_id            = "${aws_vpc.mlex.id}"
  availability_zone = "us-east-1a"
  cidr_block        = "10.0.0.0/24"
  tags = {
    Name    = "MLex Public A",
    AssetID = "2652"
  }
}

resource "aws_subnet" "mlex-public-b" {
  vpc_id            = "${aws_vpc.mlex.id}"
  availability_zone = "us-east-1b"
  cidr_block        = "10.0.1.0/24"
  tags = {
    Name    = "MLex Public B",
    AssetID = "2652"
  }
}

resource "aws_route_table" "mlex-public" {
  vpc_id = "${aws_vpc.mlex.id}"
  tags = {
    Name    = "MLex Public",
    AssetID = "2652"
  }
}

resource "aws_route" "mlex-igw" {
  route_table_id         = "${aws_route_table.mlex-public.id}"
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = "${aws_internet_gateway.mlex.id}"
}

resource "aws_route_table_association" "mlex" {
  subnet_id      = "${aws_subnet.mlex-public-a.id}"
  route_table_id = "${aws_route_table.mlex-public.id}"
}

resource "aws_security_group" "mlex-lb-api" {
  name        = "MLex API ALB"
  description = "No external access to build servers"
  vpc_id      = "${aws_vpc.mlex.id}"
  tags = {
    Name    = "MLex API ALB",
    AssetID = "2652"
  }
}

resource "aws_security_group_rule" "mlex-lb-api-https" {
  type              = "ingress"
  from_port         = 443
  to_port           = 443
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = "${aws_security_group.mlex-lb-api.id}"
}


resource "aws_security_group_rule" "mlex-lb-api-out-all" {
  type              = "egress"
  from_port         = 0
  to_port           = 65535
  protocol          = "-1"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = "${aws_security_group.mlex-lb-api.id}"
}

resource "aws_alb" "mlex-api" {
  name = "MLex-API"
  internal = false
  load_balancer_type = "application"
  security_groups = ["${aws_security_group.mlex-lb-api.id}"]
  subnets = ["${aws_subnet.mlex-public-a.id}", "${aws_subnet.mlex-public-b.id}"]
  tags = {
    Name    = "MLex API",
    AssetID = "2652"
  }
}
