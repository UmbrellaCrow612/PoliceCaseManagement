package grpcmiddleware

import (
	"context"
	"log"

	"google.golang.org/grpc"
)

// Public: LogGrpc logs the gRPC method being called and any resulting error.
func LogGrpc(
	ctx context.Context,
	req any,
	info *grpc.UnaryServerInfo,
	handler grpc.UnaryHandler,
) (any, error) {
	log.Printf("gRPC call: %s", info.FullMethod)

	resp, err := handler(ctx, req)

	if err != nil {
		log.Printf("gRPC error: %v", err)
	}

	return resp, err
}
