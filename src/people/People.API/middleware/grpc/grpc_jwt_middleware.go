package grpcmiddleware

import (
	"context"
	"strings"

	"github.com/golang-jwt/jwt/v5"
	"google.golang.org/grpc"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/metadata"
	"google.golang.org/grpc/status"
)

// A private key for context that is guaranteed to be unique.
type contextKey struct{}

// The key for storing claims in context.
var claimsContextKey = contextKey{}

// JwtMiddleware creates a new gRPC UnaryServerInterceptor that validates a JWT.
// It checks for a 'Bearer' token in the 'authorization' metadata.
// If the token is valid, it adds the JWT claims to the request context.
//
// Parameters:
//   - secret: The secret key (string) used to sign the JWT.
//   - issuer: The required 'iss' (issuer) claim.
//   - audience: The required 'aud' (audience) claim.
func JwtMiddleware(secret, issuer, audience string) grpc.UnaryServerInterceptor {
	return func(
		ctx context.Context,
		req any,
		info *grpc.UnaryServerInfo,
		handler grpc.UnaryHandler,
	) (any, error) {
		md, ok := metadata.FromIncomingContext(ctx)
		if !ok {
			return nil, status.Error(codes.Unauthenticated, "metadata is not provided")
		}

		authHeader := md["authorization"]
		if len(authHeader) < 1 {
			return nil, status.Error(codes.Unauthenticated, "authorization header is not provided")
		}

		tokenString := strings.TrimPrefix(authHeader[0], "Bearer ")
		if tokenString == authHeader[0] {
			return nil, status.Error(codes.Unauthenticated, "authorization header format must be 'Bearer <token>'")
		}

		claims, err := validateJwt(tokenString, secret, issuer, audience)
		if err != nil {
			return nil, status.Error(codes.Unauthenticated, err.Error())
		}

		newCtx := context.WithValue(ctx, claimsContextKey, claims)

		return handler(newCtx, req)
	}
}

// validateJwt parses and validates a JWT string.
// It verifies the signing method, signature, and standard claims (exp, iss, aud).
//
// Returns the parsed claims or an error if validation fails.
func validateJwt(tokenStr, sec, iss, aud string) (*jwt.RegisteredClaims, error) {
	claims := &jwt.RegisteredClaims{}

	token, err := jwt.ParseWithClaims(tokenStr, claims, func(token *jwt.Token) (interface{}, error) {
		// Make sure the signing method is what we expect.
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, status.Errorf(codes.Unauthenticated, "unexpected signing method: %v", token.Header["alg"])
		}
		return []byte(sec), nil
	}, jwt.WithIssuer(iss), jwt.WithAudience(aud))

	if err != nil {
		return nil, err
	}

	if !token.Valid {
		return nil, status.Error(codes.Unauthenticated, "token is not valid")
	}

	return claims, nil
}

func ClaimsFromContext(ctx context.Context) (*jwt.RegisteredClaims, bool) {
	claims, ok := ctx.Value(claimsContextKey).(*jwt.RegisteredClaims)
	return claims, ok
}
