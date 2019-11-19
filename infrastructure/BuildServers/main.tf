resource "aws_vpc" "build" {
  cidr_block = "11.0.0.0/16"
  tags = {
    Name    = "Build Servers",
    AssetID = "2652"
  }
}

resource "aws_internet_gateway" "build" {
  vpc_id = "${aws_vpc.build.id}"
  tags = {
    Name    = "Build Servers",
    AssetID = "2652"
  }
}

resource "aws_subnet" "build-a" {
  vpc_id            = "${aws_vpc.build.id}"
  availability_zone = "us-east-1a"
  cidr_block        = "11.0.0.0/24"
  tags = {
    Name    = "Build Servers A",
    AssetID = "2652"
  }
}

resource "aws_route_table" "build" {
  vpc_id = "${aws_vpc.build.id}"
  tags = {
    Name    = "Build Servers",
    AssetID = "2652"
  }
}

resource "aws_route" "build-igw" {
  route_table_id         = "${aws_route_table.build.id}"
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = "${aws_internet_gateway.build.id}"
}

resource "aws_route_table_association" "build" {
  subnet_id      = "${aws_subnet.build-a.id}"
  route_table_id = "${aws_route_table.build.id}"
}

resource "aws_security_group" "build-servers" {
  name        = "Build Servers"
  description = "No external access to build servers"
  vpc_id      = "${aws_vpc.build.id}"
  tags = {
    Name    = "Build Servers",
    AssetID = "2652"
  }
}

resource "aws_security_group_rule" "out-all" {
  type              = "egress"
  from_port         = 0
  to_port           = 65535
  protocol          = "-1"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = "${aws_security_group.build-servers.id}"
}

