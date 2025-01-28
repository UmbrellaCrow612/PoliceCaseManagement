import { useState } from "react";
import { Button, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import {
  CameraView,
  CameraType,
  useCameraPermissions,
  BarcodeScanningResult,
} from "expo-camera";

export default function ScanTab() {
  const [back, _] = useState<CameraType>("back");
  const [permission, requestPermission] = useCameraPermissions();
  const [barCodeData, setBarcodeData] = useState("");

  function onBarcodeScanned(scanningResult: BarcodeScanningResult) {
    // Tis is what the identity QR OTP Code will be we will prase it validate it then push the data to stateless otp view which renders the data into it
    setBarcodeData(scanningResult.data);
  }

  if (!permission || !permission.granted) {
    return (
      <View>
        <Text>Camera doesn't have permission, please grant it</Text>
        <Button title="Request permission" onPress={requestPermission} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <CameraView
        style={styles.camera}
        facing={back}
        barcodeScannerSettings={{
          barcodeTypes: ["qr"],
        }}
        onBarcodeScanned={onBarcodeScanned}
      >
        <View style={styles.overlay}>
          <View style={styles.roundedBox}>
            <Text style={styles.title}>Scan QR Code Data : {barCodeData}</Text>
          </View>
        </View>
      </CameraView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  camera: {
    flex: 1,
  },
  overlay: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "transparent",
  },
  roundedBox: {
    width: 250,
    height: 250,
    borderRadius: 20,
    borderWidth: 2,
    borderColor: "white",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(255, 255, 255, 0.1)",
  },
  title: {
    color: "white",
    fontSize: 20,
    fontWeight: "bold",
    marginTop: 10,
  },
});
